using Microsoft.AspNetCore.Authentication;
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
        [Remote("IsUserExist","Account", HttpMethod = "POST", AdditionalFields = "__RequestVerificationToken")]
        public string UserName { get; set; }
        [Required()]
        [Display(Name ="ایمیل")]
        [EmailAddress]
        [MaxLength(300)]
        [Remote("VeryfyEmail", "Account",HttpMethod ="POST",AdditionalFields = "__RequestVerificationToken")]

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
        [Required(ErrorMessage ="شماره تلفن خود را وارد کنین")]
        [Display(Name ="لطفا  شماره تلفن خود را وارد کنین")]
        public string PhoneNumber { get; set; }
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

        public string ReurnUrl { get; set; }
        public List<AuthenticationScheme> ExternalLogin { get; set; }


    }
}
