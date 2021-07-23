using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LLVBog.Models;
using System.Net;
using System.Net.Mail;

namespace LLVBog.Controllers
{
    public class ContactUsController : Controller
    {
        private BlogDataEntities _db = new BlogDataEntities();

        // GET: ContactUs
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ContactUs contactUs)
        {
            if (ModelState.IsValid)
            {
                SendMailContactBlog(contactUs);
                ViewBag.Message = "Blog sẽ phản hồi lại cho bạn trong khoản thời gian sớm nhất";
                return View();
            }
            return View(contactUs);
        }

        private void SendMailContactBlog(ContactUs contactUs)
        {

            using (MailMessage mm = new MailMessage("hvlshopresetpass@gmail.com", "hvlshopresetpass@gmail.com"))
            {
                mm.Subject = "Liên hệ từ Blog";
                string body = "";
                if (contactUs.Username == null)
                {
                    body = "Gửi từ người dùng hệ thống chưa đăng nhập,";
                }
                else { body = "Gửi từ User " + contactUs.Username + ","; }
                body += "<br /><br />Email: " + contactUs.Gmail;
                body += "<br /><br />" + contactUs.Message;
                mm.Body = body;
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("hvlshopresetpass@gmail.com", "Long20121966");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
            }
        }
    }
}