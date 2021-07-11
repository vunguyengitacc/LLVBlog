using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LLVBog.Models
{
    public class ChangePassword
    {

        [Required(ErrorMessage = "Không được để trống", AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength =8, ErrorMessage ="Mật khẩu nên trong khoảng từ 8 đến 20 kí tự")]
        [RegularExpression("[a-zA-Z0-9]+", ErrorMessage = "Mật khẩu chỉ nên chứa kí tự số hoặc chữ cái")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        public String oldPassword {get; set;}

        [Required(ErrorMessage = "Không được để trống", AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Mật khẩu nên trong khoảng từ 8 đến 20 kí tự")]
        [RegularExpression("[a-zA-Z0-9]+", ErrorMessage = "Mật khẩu chỉ nên chứa kí tự số hoặc chữ cái")]
        [DataType(DataType.Password)]        
        [Display(Name ="Mật khẩu mới")]
        public String newPassword { get; set; }


        [Required(ErrorMessage = "Không được để trống", AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "Mật khẩu nên trong khoảng từ 8 đến 20 kí tự")]
        [RegularExpression("[a-zA-Z0-9]+", ErrorMessage = "Mật khẩu chỉ nên chứa kí tự số hoặc chữ cái")]
        [DataType(DataType.Password)]
        [Compare("newPassword", ErrorMessage = "Mã xác nhận không chính xác")]
        [Display(Name = "Xác nhận")]
        public String confirmPassword { get; set; }
    }
}