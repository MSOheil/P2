using Microsoft.EntityFrameworkCore;
using RShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RShop.Data.Repositories
{
   public interface IHomeControllerRepository
    {
        IEnumerable<Product> GetAllProduct();

    }
    public class HomeControllerRepository : IHomeControllerRepository
    {
        private RShopContext_DB _context;
        public HomeControllerRepository(RShopContext_DB context)
        {
            _context = context;
        }
        public IEnumerable<Product> GetAllProduct()
        {
            return _context.Products.ToList();
        }

   
    }
}
