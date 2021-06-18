using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RShop.Data;
using RShop.Models;

namespace RShop.Pages.Shared.Admin
{
    public class IndexModel : PageModel
    {

        private RShopContext_DB _context;
        public IndexModel( RShopContext_DB context)
        {
            _context = context;
        }
        public IEnumerable<Product> Products { get; set; }
        public void OnGet()
        {
            Products = _context.Products.ToList();
        }
        public void OnPost()
        {

        }
    }
}
