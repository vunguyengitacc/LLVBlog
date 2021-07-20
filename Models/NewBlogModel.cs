using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using LLVBog.Models;
namespace LLVBog.Models
{
    public class NewBlogModel
    {

        public int Id { get; set; }
        [Required]
        [StringLength(2000, MinimumLength = 100, ErrorMessage = "Tối thiểu 100 kí tự")]
        public string Content { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "Tối đa 250 kí tự")]
        [Display(Name ="Tiêu đề")]
        public string Title { get; set; }        

        [Display(Name = "Chọn thể loại bài viết")]
        public List<Category> Categories { get; set; }

        public List<int> ThisCategories { get; set; }
    }
}