using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using LLVBog.Models;


namespace LLVBog.Controllers
{
    public class ProfileController : Controller
    {
        private BlogDataEntities db = new BlogDataEntities();

        // GET: Profile
        public ActionResult Me(int? page = 1)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Home");
            String acc = Session["Username"].ToString();
            Account temp = db.Accounts.Where(i => i.Username == acc).FirstOrDefault();
            if (temp == null)
                return RedirectToAction("Index", "Home");
            HashSet<String> lstCategory = new HashSet<String>();
            temp.Blogs.ToList().ForEach(i => i.Categories.ToList().ForEach(si => lstCategory.Add(si.Name)));

            //Phân trang
            int amount = temp.Blogs.Count();
            int pageMax = amount % 5 == 0 ? amount / 5 : amount / 5 + 1; // lấy stt trang lớn nhất
            if (page > pageMax)
                page = pageMax; //nếu parameter page > stt lớn nhất => gán page = trang lớn nhất

            temp.Blogs = temp.Blogs.Skip(5 * ((int)page - 1)).Take(5).ToList();


            ViewBag.PageMax = pageMax;
            ViewBag.PageID = (int)page;

            ViewBag.ListCategory = lstCategory;
            return View(temp);
        }

        public ActionResult Author(String username, int? page=1)
        {
            if (ModelState.IsValid)
            {
                Account acc = db.Accounts.Where(i => i.Username == username).FirstOrDefault();
                if (acc == null)
                    return View("Error");
                HashSet<String> lstCategory = new HashSet<String>();
                acc.Blogs.ToList().ForEach(i => i.Categories.ToList().ForEach(si => lstCategory.Add(si.Name)));
                acc.Blogs.OrderByDescending(i => i.CreatedDate);


                int amount = acc.Blogs.Count();
                int pageMax = amount % 5 == 0 ? amount / 5 : amount / 5 + 1; // lấy stt trang lớn nhất
                if (page > pageMax)
                    page = pageMax; //nếu parameter page > stt lớn nhất => gán page = trang lớn nhất
                acc.Blogs = acc.Blogs.Skip(5 * ((int)page - 1)).Take(5).ToList();


                ViewBag.PageMax = pageMax;
                ViewBag.PageID = (int)page;

                ViewBag.ListCategory = lstCategory;
                return View(acc);
            }
            else return View("Error");
        }

        public ActionResult CreateNew()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Home");
            String acc = Session["Username"].ToString();
            Account temp = db.Accounts.Where(i => i.Username == acc).FirstOrDefault();
            if (temp == null)
                return RedirectToAction("Index", "Home");
            NewBlogModel blog = new NewBlogModel();
            blog.Categories = db.Categories.ToList();
            return View(blog);

        }

        [HttpPost]
        public JsonResult CreateNew(NewBlogModel newItem)
        {
            if (Session["Username"] == null)
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            String acc = Session["Username"].ToString();            
            Account temp = db.Accounts.Where(i => i.Username == acc).FirstOrDefault();

            if (temp == null)
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);

            if (ModelState.IsValid)
            {
                Blog newBlog = new Blog
                {
                    Title = newItem.Title,
                    Content = newItem.Content,
                    Account = temp,
                    CreatedDate = DateTime.Now,
                    TotalView = 0                    
                };
                if (newItem.Categories.Count() == 0)
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                newItem.ThisCategories.ForEach(item =>
                {
                    Category tempCategory = db.Categories.Where(i => i.CategoryId == item).FirstOrDefault();
                    if (tempCategory != null)
                        newBlog.Categories.Add(tempCategory);
                });
                db.Blogs.Add(newBlog);                
                db.SaveChanges();
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = false }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Edit(int? id)
        {
            if(Session["Username"] == null)
                return RedirectToAction("Index", "Home");
            String acc = Session["Username"].ToString();
            Account temp = db.Accounts.Where(i => i.Username == acc).FirstOrDefault();
            if (temp == null)
                return RedirectToAction("Index", "Home");
            NewBlogModel blog = new NewBlogModel();
            Blog tempBlog = db.Blogs.Where(item => item.BlogId == id).FirstOrDefault();
            if(tempBlog == null)
                return RedirectToAction("Index", "Home");
            blog.Content = tempBlog.Content;
            blog.Title = tempBlog.Title;
            blog.Id = tempBlog.BlogId;
            blog.ThisCategories = tempBlog.Categories.Select(i => i.CategoryId).ToList();
            blog.Categories = db.Categories.ToList();
            return View(blog);

        }

        [HttpPost]
        public JsonResult Edit(int? id ,NewBlogModel edited)
        {
            if (!ModelState.IsValid)
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            if (Session["Username"] == null)
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            String acc = Session["Username"].ToString();
            Account temp = db.Accounts.Where(i => i.Username == acc).FirstOrDefault();
            if (temp == null)
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            Blog targetItem = db.Blogs.Where(i => i.BlogId == edited.Id).FirstOrDefault();
            if(targetItem == null)
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            targetItem.Content = edited.Content;
            targetItem.Title = edited.Title;
            targetItem.UpdatedDate = DateTime.Now;
            targetItem.Categories = new List<Category>();
            if(edited.Categories.Count()==0)
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            edited.ThisCategories.ForEach(item =>
            {
                Category tempCategory = db.Categories.Where(i => i.CategoryId == item).FirstOrDefault();
                if (tempCategory != null)
                    targetItem.Categories.Add(tempCategory);
            });
            db.SaveChanges();
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChangePass()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Home");
            String acc = Session["Username"].ToString();
            Account temp = db.Accounts.Where(i => i.Username == acc).FirstOrDefault();
            if (temp == null)
                return RedirectToAction("Index", "Home");
            ChangePassword handler = new ChangePassword();
            return View(handler);
        }


        [HttpPost]
        public ActionResult ChangePass(ChangePassword changeInfor)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Home");
            if (ModelState.IsValid)
            {

                String acc = Session["Username"].ToString();
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
            else
            {                
                return View(changeInfor);
            }
        }

        public ActionResult Detail()
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Home");
            String acc = Session["Username"].ToString();
            Account temp = db.Accounts.Where(i => i.Username == acc).FirstOrDefault();
            if (temp == null)
                return View("Error");
            return View(temp);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Detail(FormCollection form)
        {
            if (Session["Username"] == null)
                return RedirectToAction("Index", "Home");
            String acc = Session["Username"].ToString();
            Account temp = db.Accounts.Where(i => i.Username == acc).FirstOrDefault();
            if (temp == null)
                return RedirectToAction("Index", "Home");
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

    }
}