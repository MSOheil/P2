using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RShop.Data
{
    public class RShopContext_DB : IdentityDbContext
    {
        public RShopContext_DB(DbContextOptions<RShopContext_DB> options) : base(options)
        {

        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategorytpProduct> CategorytpProducts { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CategorytpProduct>().HasKey(p => new { p.CategoryID, p.ProductID });

            #region SeedData Category
            modelBuilder.Entity<Category>().HasData(new Category()
            {
                ID = 1,
                Name = "لباس ورزشی",
                Description = "انواع لباس برای ورزش"
            },
            new Category()
            {
                ID = 2,
                Name = "لباس های مزدانه",
                Description = "لباس های مردانه با قیمت عالی"
            },
            new Category()
            {
                ID = 3,
                Name = "لباس های بزرگ",
                Description = " تیشرت های بزرگ برای همه سنین "
            });
            modelBuilder.Entity<Product>().HasData(new Product() {
                ID = 1,
                Name = "شلوار لی مردانه",
                Description = "انواع شلوار لی با سایز های بزرگ و کوچک",
                Price = 45000M,
                Quantity = 43
            },
            new Product()
            {
                ID = 2,
                Name = "لباس  ورزشی",
                Description = "لباس های ورزشی مخصوص انواع رشته ها",
                Price = 45000M,
                Quantity = 43
            },
            new Product()
            {
                ID = 3,
                Name = "تیشرت مردانه",
                Description = "انواع تیشرت با سایز های بزرگ  و کوچک",
                Price = 45000M,
                Quantity = 43
            });
            modelBuilder.Entity<CategorytpProduct>().HasData(
                new CategorytpProduct() { CategoryID = 1, ProductID = 1 },
                new CategorytpProduct() { CategoryID = 2, ProductID = 1 },
                new CategorytpProduct() { CategoryID = 3, ProductID = 1 },
                new CategorytpProduct() { CategoryID = 1, ProductID = 2 },
                new CategorytpProduct() { CategoryID = 2, ProductID = 2 },
                new CategorytpProduct() { CategoryID = 3, ProductID = 2 },
                new CategorytpProduct() { CategoryID = 1, ProductID = 3 },
                new CategorytpProduct() { CategoryID = 2, ProductID = 3 },
                new CategorytpProduct() { CategoryID = 3, ProductID = 3 }
                );
            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
