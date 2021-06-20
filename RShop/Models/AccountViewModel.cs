using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RShop.Models
{
    public class RegisterViewModel
    {

        [Required]
        [Display(Name ="نام کاربری ")]
        public string UserName { get; set; }
        [Required()]
        [Display(Name ="ایمیل")]
        [EmailAddress]
        [MaxLength(300)]
        [Remote("VeryfyEmail", "Account")]

        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name ="رمز عبور")]
        [MaxLength(50)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = " تکرار زمز عبور")]
        [MaxLength(50)]
        [Compare("Password")]
        public string RePassword { get; set; }
    }
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "نام کاربری ")]
        public string UserName { get; set; }
        [Required()]
        [Display(Name = "ایمیل")]
        [EmailAddress]
        [MaxLength(300)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور")]
        [MaxLength(50)]
        public string Password { get; set; }
        [Display(Name ="مزابه خاطر بسپار")]
        public bool RememberMe { get; set; }
    }
}
