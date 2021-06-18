using Microsoft.EntityFrameworkCore;
using RShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RShop.Data.Repositories
{
    
   public interface IGroupRepository
    {

        IEnumerable<Category> GetAllCategories();
        IEnumerable<ShowGroupViewModel> GetGroupForShow();
    }
    public class GroupRepository : IGroupRepository
    {
        private RShopContext_DB _context;
        public GroupRepository(RShopContext_DB context)
        {
            _context = context;
        }
        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories.ToList();

        }

        public IEnumerable<ShowGroupViewModel> GetGroupForShow()
        {
            return _context.Categories.Select(p => new ShowGroupViewModel()
            {
                ID = p.ID,
                GroupName = p.Name,
                ProductCount = _context.CategorytpProducts.Count(ca => ca.CategoryID == p.ID)
            }); 
        }
    }
}
