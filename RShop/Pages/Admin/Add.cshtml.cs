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
    public class AddModel : PageModel
    {
        private RShopContext_DB _context;
        public AddModel(RShopContext_DB context)
        {
            _context = context;
        }
        [BindProperty]
        public AddEditViewModel Product { get; set; }
        [BindProperty]
        public List<int> selectedGroups { get; set; }
        public void OnGet()
        {
            Product = new AddEditViewModel()
            {
                Categories = _context.Categories.ToList()
            };


        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            var pro = new Product()
            {
                Name = Product.Name,
                Description = Product.Description,
                Price = Product.Price,
                Quantity = Product.Count
            };
            _context.Products.Add(pro);
            _context.SaveChanges();

            if(Product.Picture?.Length>0)
            {
                string filepath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    pro.ID + Path.GetExtension(Product.Picture.FileName));
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    Product.Picture.CopyTo(stream);
                }
            }
            if(selectedGroups.Any() && selectedGroups.Count>0)
            {
                foreach (int gr in selectedGroups)
                {
                    _context.CategorytpProducts.Add(new CategorytpProduct()
                    {
                        CategoryID=gr,
                        ProductID=pro.ID
                    });
                }
                _context.SaveChanges();
            }




            return RedirectToPage("Index");
        }
    }
}
