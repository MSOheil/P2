using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RShop.Data;
using RShop.Models;

namespace RShop.Pages.Admin
{
    public class DeleteModel : PageModel
    {
        private RShopContext_DB _context;
        public DeleteModel(RShopContext_DB context)
        {
            _context = context;
        }
        [BindProperty]
        public Product Product { get; set; }
        public void OnGet(int id)
        {
            Product = _context.Products.FirstOrDefault(sa => sa.ID == id);
               

        }
        public IActionResult OnPost()
        {
            var product = _context.Products.Find(Product.ID);
            _context.Products.Remove(product);

            _context.SaveChanges();
            string filepath = Path.Combine(Directory.GetCurrentDirectory(),
                   "wwwroot",
                   "images",
                   product.ID + ".jpg");
            if(System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
            return RedirectToPage("Index");
        }
    }
}
