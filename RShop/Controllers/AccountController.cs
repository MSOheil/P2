using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RShop.Data.Repositories;
using RShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RShop.Controllers
{
    public class AccountController : Controller
    {
        private IUserRepository _userRepository;
        private UserManager<IdentityUser> _userManager;
        public AccountController(IUserRepository userRepository, UserManager<IdentityUser> userManger)
        {
            _userManager = userManger;
            _userRepository = userRepository;
        }
        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {

            if (ModelState.IsValid)
            {
                var users = new IdentityUser()
                {
                    UserName = register.UserName,
                    Email = register.Email,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(users, register.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index","Home");

                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(register);
        }

        public IActionResult VeryfyEmail(string email)
        {
            if (_userRepository.IsExixtByEmail(email.ToLower()))
            {
                return Json($"ایمیل {email}تکراری است");
            };
            return Json(true);
        }
        #endregion

        #region Login
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var _user = _userRepository.GetUserForLogin(login.Email.ToLower(), login.Password);
            if (_user == null)
            {
                ModelState.AddModelError("Email", "اطلاعات صحیح نیست");
                return View(login);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _user.UserID.ToString()),
                new Claim(ClaimTypes.Name, _user.Email),
                new Claim("IsAdmin", _user.IsAdmin.ToString()),
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            var properties = new AuthenticationProperties()
            {
                IsPersistent = login.RememberMe
            };

            HttpContext.SignInAsync(principal, properties);

            return Redirect("/");
        }

        #endregion Login
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
