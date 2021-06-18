using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RShop.Data;
using RShop.Models;

namespace RShop.Pages.Admin.MangeUser
{
    public class IndexModel : PageModel
    {
        private readonly RShop.Data.RShopContext_DB _context;

        public IndexModel(RShop.Data.RShopContext_DB context)
        {
            _context = context;
        }

        public IList<Users> Users { get;set; }

        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();
        }
    }
}
