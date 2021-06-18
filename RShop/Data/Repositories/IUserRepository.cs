using RShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RShop.Data.Repositories
{
   public interface IUserRepository
    {
        bool IsExixtByEmail(string email);
        void AddUser(Users user);
        Users GetUserForLogin(string email,string password);

    }
    public class UserRepository : IUserRepository
    {
        private RShopContext_DB _context;
        public UserRepository(RShopContext_DB context)
        {
            _context = context;
        }
        public void AddUser(Users user)
        {
            _context.Add(user);
            _context.SaveChanges(); 
        }



        public Users GetUserForLogin(string email, string password)
        {
            return _context.Users.SingleOrDefault(e => e.Email == email && e.Password == password);
        }

        public bool IsExixtByEmail(string email)
        {
            return _context.Users.Any(s => s.Email == email);
        }
    }
}
