using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RShop.Data;
using RShop.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RShop.Controllers
{
    public class ProductController : Controller
    {
        private RShopContext_DB _context;
        public ProductController(RShopContext_DB context)
        {
            _context = context;
        }
        [Route("/Group/{id}/{name}")]
        public IActionResult ShoProductByGroup(int id, string name)
        {
            ViewData["Name"] = name;
            var products = _context.CategorytpProducts
                .Where(c => c.CategoryID == id)
                .Include(c => c.Product)
                .Select(c => c.Product)
                .ToList();
            return View("/Views/Component/ShowProductByGroup.cshtml", products);
        }
    }
}
