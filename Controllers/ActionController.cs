using System.Linq;
using System.Web.Mvc;
using LLVBog.Models;


namespace LLVBog.Controllers
{
    public class ActionController : Controller
    {
        public BlogDataEntities db = new BlogDataEntities();

        [HttpPost]
        public JsonResult ToggleVoteUp(int? blogId)
        {
            if (Session["Username"] == null)
                return Json(new { result = false, type=1 }, JsonRequestBehavior.AllowGet);
            Blog tempBlog = db.Blogs.FirstOrDefault(item => item.BlogId == blogId);
            if(tempBlog == null)
                return Json(new { result = false, type = 2 }, JsonRequestBehavior.AllowGet);
            string user = Session["Username"].ToString();
            Account tempAcc = db.Accounts.FirstOrDefault(item => item.Username == user);
            if(tempAcc == null)
                return Json(new { result = false, type = 2 }, JsonRequestBehavior.AllowGet);
            LLVBog.Models.Action action = db.Actions.FirstOrDefault(item => item.Username == user && item.BlogId == blogId);
            if (action == null)
            {
                action = new Models.Action
                {
                    Account = tempAcc,
                    Blog = tempBlog,
                    Report = false,
                    View = 0
                };
                db.Actions.Add(action);
                db.SaveChanges();
            }
            if (action.Vote != true)
                action.Vote = true;
            else
                action.Vote = null;
            db.SaveChanges();
            return Json(new { result = true, vote = db.Actions.Where(item=>item.BlogId==blogId && item.Vote == true).ToList().Count() - db.Actions.Where(item => item.BlogId == blogId && item.Vote == false).ToList().Count() }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ToggleVoteDown(int? blogId)
        {
            if (Session["Username"] == null)
                return Json(new { result = false, type = 1 }, JsonRequestBehavior.AllowGet);
            Blog tempBlog = db.Blogs.FirstOrDefault(item => item.BlogId == blogId);
            if (tempBlog == null)
                return Json(new { result = false, type = 2 }, JsonRequestBehavior.AllowGet);
            string user = Session["Username"].ToString();
            Account tempAcc = db.Accounts.FirstOrDefault(item => item.Username == user);
            if (tempAcc == null)
                return Json(new { result = false, type = 2 }, JsonRequestBehavior.AllowGet);
            LLVBog.Models.Action action = db.Actions.FirstOrDefault(item => item.Username == user && item.BlogId == blogId);
            if (action == null)
            {
                action = new Models.Action
                {
                    Account = tempAcc,
                    Blog = tempBlog,
                    Report = false,
                    View = 0
                };
                db.Actions.Add(action);
                db.SaveChanges();
            }
            if (action.Vote != false)
                action.Vote = false;
            else
                action.Vote = null;
            db.SaveChanges();
            return Json(new { result = true, vote = db.Actions.Where(item => item.BlogId == blogId && item.Vote == true).ToList().Count() - db.Actions.Where(item => item.BlogId == blogId && item.Vote == false).ToList().Count() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Report(int? blogId)
        {
            if (Session["Username"] == null)
                return Json(new { result = false, type = 1 }, JsonRequestBehavior.AllowGet);
            Blog tempBlog = db.Blogs.FirstOrDefault(item => item.BlogId == blogId);
            if (tempBlog == null)
                return Json(new { result = false, type = 2 }, JsonRequestBehavior.AllowGet);
            string user = Session["Username"].ToString();
            Account tempAcc = db.Accounts.FirstOrDefault(item => item.Username == user);
            if (tempAcc == null)
                return Json(new { result = false, type = 2 }, JsonRequestBehavior.AllowGet);
            LLVBog.Models.Action action = db.Actions.FirstOrDefault(item => item.Username == user && item.BlogId == blogId);
            if (action == null)
            {
                action = new Models.Action
                {
                    Account = tempAcc,
                    Blog = tempBlog,
                    Report = true,
                    View = 0
                };
                db.Actions.Add(action);
                db.SaveChanges();
            }
            if (action.Vote != true)
                action.Vote = true;            
            db.SaveChanges();
            return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        }
    }
}