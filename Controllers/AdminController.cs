using LLVBog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LLVBog.Controllers
{
    public class AdminController : Controller
    {
        public BlogDataEntities db = new BlogDataEntities();

        // GET: admin
        public ActionResult Statis()
        {

            List<Blog> ListBlog = new List<Blog>();

            ListBlog = db.Blogs.OrderByDescending(s => s.TotalView).ToList();
            return View(ListBlog);
        }
        public ActionResult Index()
        {
            List<Account> ListAccount = new List<Account>();
            ListAccount = db.Accounts.ToList();
            return View(ListAccount);
        }
        public ActionResult Post()
        {
            List<LLVBog.Models.Action> ListAction = new List<LLVBog.Models.Action>();
            ListAction = db.Actions.ToList();

            return View(ListAction);
        }
    }
}