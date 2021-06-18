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
    public class DetailsModel : PageModel
    {
        private readonly RShop.Data.RShopContext_DB _context;

        public DetailsModel(RShop.Data.RShopContext_DB context)
        {
            _context = context;
        }

        public Users Users { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Users = await _context.Users.FirstOrDefaultAsync(m => m.UserID == id);

            if (Users == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
