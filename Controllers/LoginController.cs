using LLVBog.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LLVBog.Controllers
{
    public class LoginController : Controller
    {

        private BlogDataEntities _db = new BlogDataEntities();

        public ActionResult Index()
        {
            if (Session["Username"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }
        public ActionResult Index_Admin()
        {
            if (Session["Username"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        //POST: ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ForgetPassword(String gmail)
        {

            if (ModelState.IsValid)
            {
                var dataUser = _db.Accounts.Where(s => s.Gmail.Equals(gmail)).ToList();
                if (dataUser.Count() > 0)
                {
                    var account = _db.Accounts.Where(p => p.Gmail == gmail).FirstOrDefault();
                    if (account != null)
                    {
                        SendResetPass(account);
                        ViewBag.Message = "Gửi mail tạo lại mật khẩu thành công";
                    }
                    else
                    {
                        ViewBag.Message = "Gửi mail tạo lại mật khẩu thất bại";
                    }
                }
                else
                {
                    ViewBag.Message = "Không tồn tại mail nào tên " + gmail + " trong hệ thống";
                }
            }
            return View("Login");
        }

        //POST: login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                /*"Vui lòng kiểm tra email để kích hoạt tài khoản"*/
                var f_password = GetMD5(password);
                var dataActive = _db.AccountActivations.Where(s => s.Gmail.Equals(username) || s.Username.Equals(username) && s.ActivationCode != null).ToList();
                var dataUser = _db.Accounts.Where(s => s.Gmail.Equals(username) || s.Username.Equals(username) && s.Password.Equals(f_password) && s.RoleID == 1).ToList();
                var dataAdmin = _db.Accounts.Where(s => s.Gmail.Equals(username) || s.Username.Equals(username) && s.Password.Equals(f_password) && s.RoleID == 2).ToList();
                if (dataUser.Count() > 0 && dataActive.Count() == 0)
                {
                    //add session
                    Session["FullName"] = dataUser.FirstOrDefault().FirstName + " " + dataUser.FirstOrDefault().LastName;
                    Session["Email"] = dataUser.FirstOrDefault().Gmail;
                    Session["Username"] = dataUser.FirstOrDefault().Username;
                    return RedirectToAction("Index", "Home");
                }
                else if (dataAdmin.Count() > 0)
                {
                    //add session
                    Session["FullName"] = dataAdmin.FirstOrDefault().FirstName + " " + dataAdmin.FirstOrDefault().LastName;
                    Session["Email"] = dataAdmin.FirstOrDefault().Gmail;
                    Session["Username"] = dataAdmin.FirstOrDefault().Username;
                    return RedirectToAction("Index","Admin");
                }
                else if (dataActive.Count() > 0)
                {
                    ViewBag.Message = "Vui lòng kiểm tra mail để kích hoạt tài khoản";
                    return View("Login");
                }
                else
                {
                    ViewBag.Message = "Tài khoản hoặc mật khẩu không hợp lệ";
                    return View("Login");
                }
            }
            return View();
        }



        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Account _user)
        {

            if (ModelState.IsValid)
            {
                var checkMail = _db.Accounts.FirstOrDefault(s => s.Gmail == _user.Gmail);
                var check = _db.Accounts.FirstOrDefault(s => s.Username == _user.Username);
                if (check != null)
                {
                    ViewBag.Message = ("Đăng ký thất bại do tên đăng nhập đã tồn tại");
                    return View("Login");
                }
                else if (checkMail != null)
                {
                    ViewBag.Message = ("Đăng ký thất bại do email đã được đăng ký");
                    return View("Login");
                }
                else if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    _user.CreatedDate = DateTime.Today;
                    _user.RoleID = 1;
                    _db.Configuration.ValidateOnSaveEnabled = false;
                    _db.Accounts.Add(_user);
                    _db.SaveChanges();
                    ViewBag.Message = ("Tạo tài khoản thành công");
                    SendActivationEmail(_user);
                    return View("Login");
                }
            }
            return View("Login");


        }



        public ActionResult Activation()
        {
            ViewBag.Message = "Invalid Activation code.";
            if (RouteData.Values["id"] != null)
            {
                Guid activationCode = new Guid(RouteData.Values["id"].ToString());
                var accountActivation = _db.AccountActivations.Where(p => p.ActivationCode == activationCode).FirstOrDefault();
                if (accountActivation != null)
                {
                    _db.AccountActivations.Remove(accountActivation);
                    _db.SaveChanges();
                    ViewBag.Message = "Activation successful.";
                }
            }

            return View();
        }

        // Send active email
        private void SendActivationEmail(Account user)
        {
            Guid activationCode = Guid.NewGuid();
            _db.AccountActivations.Add(new AccountActivation
            {
                Username = user.Username,
                Gmail = user.Gmail,
                ActivationCode = activationCode
            });
            _db.SaveChanges();

            using (MailMessage mm = new MailMessage("hvlshopresetpass@gmail.com", user.Gmail))
            {
                mm.Subject = "Kích Hoạt Tài Khoản";
                string body = "Xin chào " + user.FirstName + " " + user.LastName + ",";
                body += "<br /><br />Hãy chọn vào link phía dưới để kích hoạt tài khoản của bạn";
                body += "<br /><a href = '" + string.Format("{0}://{1}/Login/Activation/{2}", Request.Url.Scheme, Request.Url.Authority, activationCode) + "'>Chọn tại đây để kích hoạt tài khoản của bạn </a>";
                body += "<br /><br />Cảm ơn";
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

        // Send Reset email
        private void SendResetPass(Account user)
        {
            Guid resetCode = Guid.NewGuid();
            _db.AccountResetPasses.Add(new AccountResetPass
            {
                Username = user.Username,
                Gmail = user.Gmail,
                ActivationCode = resetCode
            });
            _db.SaveChanges();

            using (MailMessage mm = new MailMessage("hvlshopresetpass@gmail.com", user.Gmail))
            {
                mm.Subject = "Tạo lại mật khẩu";
                string body = "Xin chào " + user.FirstName + " " + user.LastName + ",";
                body += "<br /><br />Hãy chọn vào link phía dưới để tạo lại mật khẩu của bạn";
                body += "<br /><a href = '" + string.Format("{0}://{1}/Login/ResetPass/{2}", Request.Url.Scheme, Request.Url.Authority, resetCode) + "'>Chọn tại đây để kích hoạt tài khoản của bạn </a>";
                body += "<br /><br />Cảm ơn";
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


        //ResetPass
        public ActionResult ResetPass()
        {
            if (RouteData.Values["id"] != null)
            {
                Guid ResetCode = new Guid(RouteData.Values["id"].ToString());
                AccountResetPass accountReset = _db.AccountResetPasses.Where(p => p.ActivationCode == ResetCode).FirstOrDefault();
                ResetPasswordModel model = new ResetPasswordModel();
                if (accountReset != null)
                {
                    model.ResetCode = accountReset.ActivationCode;
                    return View(model);
                }

            }
            return View();
        }

        //POST: ResetPass
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPass(ResetPasswordModel model)
        {

            var f_password = GetMD5(model.NewPassword);
            if (model != null)
            {
                var accountResetPass = _db.AccountResetPasses.Where(p => p.ActivationCode == model.ResetCode).FirstOrDefault();
                var username = accountResetPass.Username;
                var a = _db.Accounts.Where(p => p.Username == username).FirstOrDefault();
                if (accountResetPass != null)
                {
                    a.Password = f_password;
                    _db.AccountResetPasses.Remove(accountResetPass);
                    _db.SaveChanges();
                    ViewBag.Message = "Đổi mật khẩu thành công";
                }
                else { ViewBag.Message = "Đổi mật khẩu thất bại"; }
            }
            else { ViewBag.Message = "Đổi mật khẩu thất bại"; }

            return View("Login");
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }

        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }
    }
}