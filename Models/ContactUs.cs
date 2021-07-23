using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LLVBog.Models
{
    public class ContactUs
    {
        public string Username { get; set; }

        [Required(ErrorMessage = "Không bỏ trống mail", AllowEmptyStrings = false)]
        public string Gmail { get; set; }

        [Required(ErrorMessage = "Không bỏ trống tin nhắn phản hồi", AllowEmptyStrings = false)]
        public string Message { get; set; }
    }
}