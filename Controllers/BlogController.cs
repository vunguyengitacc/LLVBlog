using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LLVBog.Models;
using PagedList;

namespace LLVBog.Controllers
{
    public class BlogController : Controller
    {
        public BlogDataEntities db = new BlogDataEntities();
        
        [NonAction]
        public List<Blog> getSortedBlogs(int type)
        {
            List<Blog> lstPost = new List<Blog>();
            if (type == 4) //Xếp theo lượt vote
                lstPost = db.Blogs.Where(i=>i.isBlock!=true && i.DeletedDate == null).OrderByDescending(i => i.Actions.Where(si => si.Vote == true).Count() - i.Actions.Where(si => si.Vote == false).Count()).ToList();
            else if (type == 3)// Xếp theo lượt xem
                lstPost = db.Blogs.Where(i => i.isBlock != true && i.DeletedDate == null).OrderByDescending(i => i.TotalView).ToList();
            else if (type == 2) //Xếp theo tiêu đề
                lstPost = db.Blogs.Where(i => i.isBlock != true && i.DeletedDate == null).OrderBy(i => i.Title).ToList();
            else //Xếp theo ngày đăng
                lstPost = db.Blogs.Where(i => i.isBlock != true && i.DeletedDate == null).OrderByDescending(i => i.CreatedDate).ToList();
            return lstPost;
        }

        [HttpPost]
        public JsonResult IncreaseView(int? blogId)
        {
            if (blogId == null)
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            Blog blog = db.Blogs.Where(i => i.BlogId == blogId).FirstOrDefault();
            if (blog == null)
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            if (Session["Username"] != null)
            {
                try
                {
                    String acc = Session["Username"].ToString();
                    LLVBog.Models.Action action = db.Actions.Where(i => i.Account.Username == acc && i.Blog.BlogId == blog.BlogId).FirstOrDefault();
                    if (action == null)
                    {
                        action = new LLVBog.Models.Action() { Username = acc, BlogId = blog.BlogId, View = 0 };
                        db.Actions.Add(action);
                    }                    
                    action.View++;
                    db.SaveChanges();
                }
                catch (InvalidCastException ex)
                {
                    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
                }

            }
            blog.TotalView++;
            db.SaveChanges();
            return Json(new { result = true, view = blog.TotalView }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Content(int? id)
        {
            if (id == 0 || id == null)
                return View("Error");
            if (Session["Username"] == null)
            {   
                Blog post = db.Blogs.Where(i => i.BlogId == id && (i.isBlock!=true && i.DeletedDate==null)).FirstOrDefault();
                if (post == null)
                    return RedirectToAction("Index", "Home");
                HashSet<Blog> anotherPost = new HashSet<Blog>();
                anotherPost.Add(post);
                foreach (var item in post.Categories)
                {
                    db.Blogs.Where(i => i.Categories
                                    .Where(si => si.Name == item.Name)
                                    .Count() != 0).ToList()
                                    .ForEach(fi =>
                                    {
                                        anotherPost.Add(fi);
                                    });

                }
                anotherPost.Remove(post);
                ViewBag.anotherPost = anotherPost.OrderBy(i => i.TotalView).Take(3);
                return View(post);
            }
            else
            {
                string username = Session["Username"].ToString();
                ViewBag.UserAction = db.Actions.FirstOrDefault(item => item.BlogId == id && item.Username == username);
                Account thisAccount = db.Accounts.FirstOrDefault(i => i.Username == username);

                Blog post = db.Blogs.Where(i => i.BlogId == id).FirstOrDefault();
                if (thisAccount.Role.RoleID == 1)
                    ViewBag.IsAdminSession = true;
                else if (post.isBlock == true || post.DeletedDate == null)
                    return RedirectToAction("Index", "Home");
                HashSet<Blog> anotherPost = new HashSet<Blog>();
                anotherPost.Add(post);
                foreach (var item in post.Categories)
                {
                    db.Blogs.Where(i => i.Categories
                                    .Where(si => si.Name == item.Name)
                                    .Count() != 0).ToList()
                                    .ForEach(fi =>
                                    {
                                        anotherPost.Add(fi);
                                    });
                }
                anotherPost.Remove(post);
                ViewBag.anotherPost = anotherPost.OrderBy(i => i.TotalView).Take(3);
                return View(post);
            }

        }

        public ActionResult Index(String q, int? page = 1, int? sort = 1, int? category = 0)
        {
            if (sort == null || (sort < 0 && sort > 4))
                sort = 1;

            List<Blog> lstPost = new List<Blog>();

            if (q != null)
                lstPost = getSortedBlogs((int)sort).Where(i => i.Content.ToUpper().Contains(q.ToUpper()) || i.Title.ToUpper().Contains(q.ToUpper())).ToList();
            else
                lstPost = getSortedBlogs((int)sort);

            //Duyệt theo thể loại
            if (category != 0)
                lstPost = lstPost.Where(i => i.Categories.Where(si => si.CategoryId == category).Count() > 0).ToList();

            // lấy danh sách các bài đăng được nhiều lượt xem nhất để làm chức năng gợi ý
            List<Blog> lstHotPosts = db.Blogs.OrderByDescending(i => i.TotalView).Take(4).ToList();

            //Lấy danh sách các thể loại
            ViewBag.lstCatgories = db.Categories.OrderByDescending(i => i.Blogs.Count).Take(10).ToList();

            
            ViewBag.SortType = (int)sort;
            ViewBag.lstHotPosts = lstHotPosts;
            ViewBag.Category = category;
            ViewBag.Query = q;
            int pageNumber = (page ?? 1);

            return View(lstPost.ToPagedList(pageNumber, 5));
        }
    }




}
