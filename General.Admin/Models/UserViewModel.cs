using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace General.Admin.Models
{
    public class UserViewModel
    {
        [Display(Name = "用户名", Description = "4-20个字符。")]
        [Required(ErrorMessage = "用户名×")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "×")]
        public string UserName { get; set; }

        [Display(Name = "密码", Description = "6-20个字符。")]
        [Required(ErrorMessage = "密码×")]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "×")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}