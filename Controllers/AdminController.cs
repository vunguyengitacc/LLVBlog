using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using LLVBog.Models;
using PagedList;
using PagedList.Mvc;

namespace LLVBog.Controllers
{
    public class AdminController : Controller
    {
        public BlogDataEntities db = new BlogDataEntities();
        // GET: admin
        // HÀM ACCOUNT
        [HttpGet]
        [NonAction]
        public bool CheckAdmin()
        {

            if (Session["Username"] == null)

                return false;

            string user = Session["Username"].ToString();
            Account accSession = db.Accounts.FirstOrDefault(i => i.Username == user);
            if (accSession.Role.RoleID != 1)
            {
                return false;
            }
            else { return true; }
        }
        public ActionResult Account(string searchkey = "", int page = 1)
        {
            if (CheckAdmin() == false)
            {
                return RedirectToAction("Login", "Login");
            }

            int pageSize = 5;
            List<Account> ListAccount = new List<Account>();

            ListAccount = db.Accounts.Where(x => x.FirstName.Contains(searchkey)).OrderByDescending(m => m.CreatedDate).ToList();

            return View(ListAccount.ToPagedList(page, pageSize));

        }

        //HÀM STATISTICAL
        public ActionResult Statistical(string searchkey = "", int page = 1)
        {
            if (CheckAdmin() == false)
            {
                return RedirectToAction("Login", "Login");
            }

            List<Blog> ListBlog = new List<Blog>();
            int pageSize = 5;
            ListBlog = db.Blogs.Where(x => x.Title.Contains(searchkey)).OrderByDescending(s => s.TotalView).ToList();

            return View(ListBlog.ToPagedList(page, pageSize));

        }
        //HÀM POST
        public ActionResult Post(string searchkey = "", int page = 1)
        {
            if (CheckAdmin() == false)
            {
                return RedirectToAction("Login", "Login");
            }

            int pageSize = 5;
            List<Blog> ListBlog = new List<Blog>();
            ListBlog = db.Blogs.Where(x => x.Title.Contains(searchkey)).ToList();
            List<Blog> ListReportBlog = ListBlog.Where(item => db.Actions.Where(i => i.BlogId == item.BlogId && i.Report == true).ToList().Count() > 0).OrderByDescending(s => s.TotalView).ToList();


            return View(ListReportBlog.ToPagedList(page, pageSize));
        }
        // HÀM CATEGORY
        public ActionResult Category(string searchkey = "", int page = 1)
        {
            if (CheckAdmin() == false)
            {
                return RedirectToAction("Login", "Login");
            }
            List<Category> ListCategory = new List<Category>();
            int pageSize = 5;
            ListCategory = db.Categories.Where(x => x.Name.Contains(searchkey)).ToList();
            return View(ListCategory.ToPagedList(page, pageSize));
        }
        //HÀM EDIT CATEGORY
        public ActionResult EditCategories(int Id)
        {
            List<Category> ListCategory = new List<Category>();

            Category category = db.Categories.SingleOrDefault(p => p.CategoryId == Id);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }
        //HÀM httpPOST EDIT
        [HttpPost]
        public ActionResult EditCategories(Category category)
        {
            List<Category> ListCategory = new List<Category>();
            Category categoryUpdate = db.Categories.SingleOrDefault(p => p.CategoryId == category.CategoryId);
            if (categoryUpdate != null)
            {
                db.Categories.AddOrUpdate(category);
                db.SaveChanges();
            }
            return RedirectToAction("Category");
        }
        //HÀM DELETE CATEGORY
        public ActionResult DeleteCategories(int Id)
        {
            List<Category> ListCategory = new List<Category>();
            Category category = db.Categories.SingleOrDefault(p => p.CategoryId == Id);
            if (category != null && category.Blogs.Count()==0)
            {
                db.Categories.Remove(category);
                db.SaveChanges();
            }            
            return RedirectToAction("Category");
        }
        //ADD CATEGORY
        public ActionResult AddCategories()
        {

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCategories([Bind(Include = "CategoryId,Name,Image")] Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Category");
            }
            return View(category);
        }
    }
}