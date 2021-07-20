using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LLVBog.Models;

namespace LLVBog.Controllers
{
    public class HomeController : Controller
    {
        public BlogDataEntities db = new BlogDataEntities();

        public ActionResult Index()
        {
            List<Blog> lstNewPost = db.Blogs.OrderByDescending(i => i.CreatedDate).Take(5).ToList();
            List<Blog> lstHotPosts = db.Blogs.OrderByDescending(i => i.TotalView).Take(4).ToList();
            ViewBag.lstCatgories = db.Categories.OrderByDescending(i => i.Blogs.Count).Take(10).ToList();
            lstNewPost.ForEach(i =>
            {
                lstHotPosts.Remove(i);
            });
            ViewBag.lstHotPosts = lstHotPosts;
            return View(lstNewPost);
        }
    }
}