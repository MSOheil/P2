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
        private IMessageSender _messageSender;
        public AccountController(IUserRepository userRepository, UserManager<IdentityUser> userManger,
            SignInManager<IdentityUser> signInManager,IMessageSender messageSender)
        {
            _userManager = userManger;
            _userRepository = userRepository;
            _signinmanager = signInManager;
            _messageSender = messageSender;
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
                    var emailConfimationToken =
                        await _userManager.GenerateEmailConfirmationTokenAsync(users);
                    var emailmessage =
                        Url.Action("ConfirmMessage", "Account",
                        new { userName = users.UserName, token = emailConfimationToken },
                        Request.Scheme
                        );
                    await _messageSender.SendEmailAsync(register.Email, "Email Confirmation", emailmessage);
                    return RedirectToAction("Index", "Home");

                }
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }
            return View(register);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VeryfyEmail(string email)
        {
            var ExistEmail = await _userManager.FindByEmailAsync(email);
            if (ExistEmail == null) return Json(true);
            return Json("ایمیل وارد شده قبلا ثبت نام کرده است");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> IsUserExist(string Username)
        {
            var IsUserExist = await _userManager.FindByNameAsync(Username);
            if (IsUserExist == null) return Json(true);
            return Json("نام کاربری  وارد شده قبلا ثبت نام کرده است");
        }
        #endregion

        #region Login
        [HttpGet]
        public async Task<IActionResult> Login(string Returnurl = null)
        {
            if (_signinmanager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");


            var model = new LoginViewModel()
            {
                ReurnUrl = Returnurl,
                ExternalLogin = (await _signinmanager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            ViewData["returnurl"] = Returnurl;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login, string Returnurl = null)
        {
            if(_signinmanager.IsSignedIn(User))
                return RedirectToAction("Index", "Home");
            login.ReurnUrl = Returnurl;
            login.ExternalLogin = (await _signinmanager.GetExternalAuthenticationSchemesAsync()).ToList();


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
        [HttpPost]
        public IActionResult ExternalLogin(string Provider,string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginBack", "Account",
                new {ReturnUrl=returnUrl });
            var properties = _signinmanager.ConfigureExternalAuthenticationProperties(Provider, returnUrl);
            return new ChallengeResult(Provider, properties);


        }
        public async Task<IActionResult> ExternalLoginBack(string returnUrl = null, string remoteError = null)
        {
            returnUrl =
                (returnUrl != null && Url.IsLocalUrl(returnUrl)) ? returnUrl : Url.Content("~/");

            var loginViewModel = new LoginViewModel()
            {
                ReurnUrl = returnUrl,
                ExternalLogin = (await _signinmanager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError("", $"Error : {remoteError}");
                return View("Login", loginViewModel);
            }

            var externalLoginInfo = await _signinmanager.GetExternalLoginInfoAsync();
            if (externalLoginInfo == null)
            {
                ModelState.AddModelError("ErrorLoadingExternalLoginInfo", $"مشکلی پیش آمد");
                return View("Login", loginViewModel);
            }

            var signInResult = await _signinmanager.ExternalLoginSignInAsync(externalLoginInfo.LoginProvider,
                externalLoginInfo.ProviderKey, false, true);

            if (signInResult.Succeeded)
            {
                return Redirect(returnUrl);
            }

            var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);

            if (email != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    var userName = email.Split('@')[0];
                    user = new IdentityUser()
                    {
                        UserName = (userName.Length <= 10 ? userName : userName.Substring(0, 10)),
                        Email = email,
                        EmailConfirmed = true
                    };

                    await _userManager.CreateAsync(user);
                }

                await _userManager.AddLoginAsync(user, externalLoginInfo);
                await _signinmanager.SignInAsync(user, false);

                return Redirect(returnUrl);
            }

            ViewBag.ErrorTitle = "لطفا با بخش پشتیبانی تماس بگیرید";
            ViewBag.ErrorMessage = $"دریافت کرد {externalLoginInfo.LoginProvider} نمیتوان اطلاعاتی از";
            return View();
        }
        #endregion Login
        public async Task<IActionResult> ConfirmEmail(string userName,string token)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(token))
                return NotFound();
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return NotFound();
            var result = await _userManager.ConfirmEmailAsync(user, token);

            return Content(result.Succeeded ? "Email Confirm" : "Email not confirm");
        }

    }


}
