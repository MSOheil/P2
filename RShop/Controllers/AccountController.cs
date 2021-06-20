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
        private SignInManager<IdentityUser> _signinmanager;
        public AccountController(IUserRepository userRepository, UserManager<IdentityUser> userManger,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManger;
            _userRepository = userRepository;
            _signinmanager = signInManager;
        }
        #region Register
        [HttpGet]
        public IActionResult Register()
        {
            if (_signinmanager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
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
                    return RedirectToAction("Index", "Home");

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
        public IActionResult Login(string Returnurl)
        {
            if (_signinmanager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            ViewData["returnurl"] = Returnurl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login, string Returnurl)
        {
            if(_signinmanager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");



            if (ModelState.IsValid)
            {
                var result = await _signinmanager.PasswordSignInAsync(login.UserName, login.Password, login.RememberMe, true);
                if (result.Succeeded)
                {
                    if (string.IsNullOrEmpty(Returnurl) && Url.IsLocalUrl(Returnurl))
                        return Redirect(Returnurl);
                    return RedirectToAction("Index", "Home");
                }
                if (result.IsLockedOut)
                {
                    ViewData["Message"] = "به دلیل ورود های ناموفق حساب شما به مدت 5 دقیقه قفل شده است";
                    return View(login);
                }
                ModelState.AddModelError("", "نام کاربری یا رمز عبور اشتباه وارد شده است");
            }



            return View(login);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
           await _signinmanager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
    #endregion Login

}
