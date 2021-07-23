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
        

        [Display(Name = "Đường dẫn ảnh bìa")]
        public string ImageUrl {get; set;}

        [Required]
        [MinLength(100)]
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