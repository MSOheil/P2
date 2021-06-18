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
    public class EditModel : PageModel
    {
        private RShopContext_DB _context;
        public EditModel(RShopContext_DB context)
        {
            _context = context;
        }
        [BindProperty]
        public AddEditViewModel Product { get; set; }
        [BindProperty]
        public List<int> selectedGroups { get; set; }

        public List<int> GroupsProduct { get; set; }
        public void OnGet(int id)
        {
            Product = _context.Products.Where(p => p.ID == id)
                .Select(s => new AddEditViewModel()
                {
                    Name = s.Name,
                    Count = s.Quantity,
                    Price = s.Price,
                    Description = s.Description,


                }).FirstOrDefault();
            Product.Categories = _context.Categories.ToList();

            GroupsProduct = _context.CategorytpProducts.Where(c => c.ProductID == id)
                .Select(s => s.CategoryID).ToList();
        }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            var product = _context.Products.Find(Product.Id);

            
            product.Name = Product.Name;
            product.Description = Product.Description;
            product.Price = Product.Price;
            product.Quantity = Product.Count;

            _context.SaveChanges();
            if (Product.Picture?.Length > 0)
            {
                string filepath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images",
                    product.ID + Path.GetExtension(Product.Picture.FileName));
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    Product.Picture.CopyTo(stream);
                }
            }
            _context.CategorytpProducts.Where(c => c.ProductID == product.ID).ToList()
                .ForEach(g => _context.CategorytpProducts.Remove(g));
            if (selectedGroups.Any() && selectedGroups.Count > 0)
            {
                foreach (int gr in selectedGroups)
                {
                    _context.CategorytpProducts.Add(new CategorytpProduct()
                    {
                        CategoryID = gr,
                        ProductID = product.ID
                    });
                }
                _context.SaveChanges();
            }



            return RedirectToPage("Index");
        }
    }
}
