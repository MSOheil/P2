using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RShop.ViewModels
{
    public class ActiveCodeViewModel
    {
        [Display(Name ="کد فعال سازی")]
        [MaxLength(6,ErrorMessage =" مقدار کد فعال سازی نمیتواند بیشتر 6 کاراکتر و عدد باشید")]
        public string Code { get; set; }
    }
}
