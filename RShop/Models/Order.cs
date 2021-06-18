using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RShop.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public DateTime CreateDate { get; set; }
        public bool IsFinaly { get; set; }




        //Navigation Property
        public List<OrderDetails> OrderDetails { get; set; }
        public Users Users { get; set; }
    }
}
