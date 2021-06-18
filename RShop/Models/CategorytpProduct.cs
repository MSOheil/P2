using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RShop.Models
{
    public class CategorytpProduct
    {
        public int CategoryID { get; set; }
        public int ProductID { get; set; }

        //Navigation Prperty
        public Product Product { get; set; }
        public Category Category { get; set; }


    }
}
