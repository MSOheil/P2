using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RShop.Data;
using RShop.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private RShopContext_DB _context;

        public HomeController(ILogger<HomeController> logger, RShopContext_DB context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var pro = _context.Products.ToList();
            return View(pro);
        }
        public IActionResult Details(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            var categories = _context.Categories.ToList();
            var vm = new DetailViewModel()
            {
                Product = product,
                Categories = categories
            };

            return View(vm);
        }
        [Authorize]
        public IActionResult AddToCart(int itemid)
        {
            var product = _context.Products.SingleOrDefault(pr => pr.ID == itemid);
            if (product != null)
            {
                var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
                var order = _context.Orders.FirstOrDefault(d => d.UserID == userid && !d.IsFinaly);
                if (order != null)
                {
                    var orderDetails = _context.OrderDetails
                        .FirstOrDefault(da => da.OrderID == order.OrderID && da.ProductID == product.ID);
                    if (orderDetails != null)
                    {
                        orderDetails.Quantity += 1;
                    }
                    else
                    {
                        _context.OrderDetails.Add(new OrderDetails()
                        {
                            OrderID = order.OrderID,
                            Price = product.Price,
                            ProductID = product.ID,
                            Quantity = 1
                        });
                    }
                }
                else
                {
                    order = new Order()
                    {
                        IsFinaly = false,
                        CreateDate = DateTime.Now,
                        UserID = userid,
                    };
                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    _context.OrderDetails.Add(new OrderDetails()
                    {
                        OrderID = order.OrderID,
                        Price = product.Price,
                        ProductID = product.ID,
                        Quantity = 1
                    });
                }
                _context.SaveChanges();
            }

            return Redirect("ShowCart");
        }
        [Authorize]
        public IActionResult ShowCart()
        {
            var userid = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier).ToString());
            var order = _context.Orders.Where(sa => sa.UserID == userid)
                .Include(s => s.OrderDetails)
                .ThenInclude(sa => sa.Product).FirstOrDefault();
            return View("/Views/Home/ShowCart.cshtml", order);
        }
        [Authorize]
        public IActionResult RemoveCart(int detailId)
        {
            var orderdetail = _context.OrderDetails.Find(detailId);
            _context.Remove(detailId);
            _context.SaveChanges();
            return Redirect("ShowCart");
        }

        [Route("/contactus")]
        public IActionResult Contactus()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
