using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RShop.Models
{
    public class OrderDetails
    {
        [Key]
        public int DetailID { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public Decimal Price { get; set; }
        [Required]
        public int OrderID { get; set; }
        [Required]
        public int ProductID { get; set; }


        // Navigation Property
        [ForeignKey("OrderID")]
        public Order Order { get; set; }
        [ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
