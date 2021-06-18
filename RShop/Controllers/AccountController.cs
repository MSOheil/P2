using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #region Register
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(RegisterViewModel register)
        {

            if (!ModelState.IsValid)
            {

                return View(register);
            }
            //if (_userRepository.IsExixtByEmail(register.Email.ToLower()))
            //{
            //    ModelState.AddModelError("Email", "ایمیل وارد شده قبلا ثبت نام کرده است");
            //    return View(register);
            //}
            Users user = new Users()
            {
                Email = register.Email.ToLower(),
                Password = register.Password,
                IsAdmin = false,
                RegisterDate = DateTime.Now
            };
            _userRepository.AddUser(user);
            return View("/Views/Account/SuccesssRegister.cshtml", register);
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
