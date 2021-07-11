using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LLVBog.Models;
using LLVBog.Controllers;

namespace LLVBog.Controllers
{
    public class ProfileController : Controller
    {
        private BlogDataEntities db = new BlogDataEntities();

        // GET: Profile
        public ActionResult Me()
        {
            String acc = Session["Username"].ToString();            
            if(acc != "")
            {
                Account temp = db.Accounts.Where(i => i.Username == acc).FirstOrDefault();
                if (temp == null)
                    return View("Error");
                HashSet<String> lstCategory = new HashSet<String>();
                temp.Blogs.ToList().ForEach(i => i.Categories.ToList().ForEach(si => lstCategory.Add(si.Name)));
                ViewBag.ListCategory = lstCategory;
                return View(temp);
            }
            else return View("Error");
        }

        public ActionResult Author(String username)
        {
            if (ModelState.IsValid)
            {
                Account acc = db.Accounts.Where(i => i.Username == username).FirstOrDefault();
                if (acc == null)
                    return View("Error");
                HashSet<String> lstCategory = new HashSet<String>();
                acc.Blogs.ToList().ForEach(i => i.Categories.ToList().ForEach(si => lstCategory.Add(si.Name)));
                ViewBag.ListCategory = lstCategory;
                return View(acc);
            }
            else return View("Error");
        }

        public ActionResult CreateNew()
        {
            String acc = Session["Username"].ToString();
            if (acc != "")
            {
                Account temp = db.Accounts.Where(i => i.Username == acc).FirstOrDefault();
                if (temp == null)
                    return View("Error");
                Blog blog = new Blog();
                return View(blog);
            }
            else return View("Error");
                
        }

        [HttpPost]
        public ActionResult CreateNew(Blog newItem)
        {
            if (ModelState.IsValid)
            {
                newItem.CreatedDate = new DateTime();
                newItem.TotalView = 0;
                db.Blogs.Add(newItem);
                db.SaveChanges();
            }
            return View();
        }


        public ActionResult ChangePass()
        {
            String acc = Session["Username"].ToString();
            if (acc != "")
            {
                Account temp = db.Accounts.Where(i => i.Username == acc).FirstOrDefault();
                if (temp == null)
                    return View("Error");
                ChangePassword handler = new ChangePassword();
                return View(handler);
            }
            else return View("Error");
        }


        [HttpPost]
        public ActionResult ChangePass(ChangePassword changeInfor)
        {
            if (ModelState.IsValid)
            {
                String acc = Session["Username"].ToString();
                if (acc != "")
                {
                    Account temp = db.Accounts.Where(i => i.Username == acc).FirstOrDefault();
                    if (temp == null)
                        return View("Error");
                    if (!temp.Password.Equals(LoginController.GetMD5(changeInfor.oldPassword)))
                    {
                        ModelState.AddModelError("", "Mật khẩu cũ không khớp");
                        return View(changeInfor);
                    }
                        
                    temp.Password = LoginController.GetMD5(changeInfor.newPassword);
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges();
                    return RedirectToAction("Me");
                }
                else return View("Error");
            }
            else
            {                
                return View(changeInfor);
            }
        }

        public ActionResult Detail()
        {

            String acc = Session["Username"].ToString();
            if (acc != "")
            {
                Account temp = db.Accounts.Where(i => i.Username == acc).FirstOrDefault();
                if (temp == null)
                    return View("Error");                
                return View(temp);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detail(FormCollection form)
        {
            String acc = Session["Username"].ToString();
            if (acc != "")
            {
                Account temp = db.Accounts.Where(i => i.Username == acc).FirstOrDefault();
                if (temp == null)
                    return View("Error");
                temp.FirstName = form["FirstName"];
                temp.LastName = form["LastName"];
                temp.Gmail = form["Gmail"];
                if (ModelState.IsValid) 
                {
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.SaveChanges(); 
                    return View(temp);
                }
                else
                {
                    ModelState.AddModelError("", "Mật khẩu cũ không khớp");
                    return View(temp);
                }
                
                
                
            }
            return View("Error");
        }

    }
}