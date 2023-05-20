using Eduhome.DAL;
using Eduhome.Helpers;
using Eduhome.Models;
using Eduhome.Services;
using Eduhome.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IWebHostEnvironment env, AppDbContext context, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _env = env;
            _context = context;
            _emailService = emailService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterM userRegister)
        {
            if (!ModelState.IsValid) return View();

            if (_userManager.Users.Any(x => x.NormalizedUserName == userRegister.UserName.ToUpper()))
            {
                ModelState.AddModelError("UserName", "This username is using!");
                return View();
            }

            if (_userManager.Users.Any(x => x.NormalizedEmail == userRegister.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "This email is using!");
                return View();
            }

            AppUser appUser = new AppUser
            {
                UserName = userRegister.UserName,
                Email = userRegister.Email,
                FullName = userRegister.FullName
            };

            var result = await _userManager.CreateAsync(appUser, userRegister.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            var rolResult = await _userManager.AddToRoleAsync(appUser, "Member");

            if (!rolResult.Succeeded)
            {
                foreach (var item in rolResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
            }

            await _signInManager.SignInAsync(appUser, true);

            if (userRegister.ImageFile != null)
            {
                if (userRegister.ImageFile.ContentType != "image/jpeg" && userRegister.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "You can choose file only .jpg, .jpeg or .png format!");
                    return View();
                }

                if (userRegister.ImageFile.Length > 5242880)
                {
                    ModelState.AddModelError("ImageFile", "You can choose file only maximum 5Mb !");
                    return View();
                }

                appUser.Image = FileManager.Save(_env.WebRootPath, "uploads/users", userRegister.ImageFile);
            }

            _context.SaveChanges();

            return RedirectToAction("index", "home");
        }

        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLoginM userL)
        {
            AppUser user = await _userManager.FindByNameAsync(userL.Username);

            if (user == null || user.IsAdmin)
            {
                ModelState.AddModelError("", "Username or Password is false!");
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(user, userL.Password, true, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is false!");
                return View();
            }

            return RedirectToAction("index", "home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPVM forgotPVM)
        {
            AppUser user = await _userManager.FindByEmailAsync(forgotPVM.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Email is not valid!");
                return View();
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            string callback = Url.Action("resetpassword", "account", new { token, email = user.Email }, Request.Scheme);

            string body = string.Empty;

            using(StreamReader reader = new StreamReader("wwwroot/templates/forgot-password.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{url}}", callback);

            _emailService.Send(user.Email,"Reset Password!", body);

            return View();
        }

        public IActionResult ResetPassword(string token, string email)
        {
            ResetPVM resetPVM = new ResetPVM
            {
                Token = token,
                Email = email
            };
            return View(resetPVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPVM resetPVM)
        {
            if (!ModelState.IsValid) return View(resetPVM);

            AppUser user = await _userManager.FindByEmailAsync(resetPVM.Email);

            if(user == null) return PartialView("_NotFoundPartial");

            var resetResult = await _userManager.ResetPasswordAsync(user, resetPVM.Token, resetPVM.Password);

            if (!resetResult.Succeeded)
            {
                foreach (var item in resetResult.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(resetPVM);
            }

            return RedirectToAction("login");
        }



        [Authorize(Roles = "Member")]
        public async Task<IActionResult> Edit()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            UserRegisterM userM = new UserRegisterM
            {
                FullName = user.FullName,
                Email = user.Email,
                UserName = user.UserName,
                Image = user.Image
            };

            return View(userM);
        }
         
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserRegisterM userM)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            user.FullName = userM.FullName;
            user.Email = userM.Email;
            user.UserName = userM.UserName;

            if (userM.ImageFile != null)
            {
                if (userM.ImageFile.ContentType != "image/jpeg" && userM.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Yalniz .jpg , .jpeg ve ya .png formatda fayl sece bilersiz!");
                    return View();
                }
                if (userM.ImageFile.Length > 5242880)
                {
                    ModelState.AddModelError("ImageFile", "Maksimum uzunlugu 5Mb olan fayl sece bilersiz!");
                    return View();
                }

                string newFileName = FileManager.Save(_env.WebRootPath, "uploads/users", userM.ImageFile);

                if (!string.IsNullOrWhiteSpace(user.Image))
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/users", user.Image);
                }

                user.Image = newFileName;
            }
            else if (string.IsNullOrWhiteSpace(userM.Image) && !string.IsNullOrWhiteSpace(user.Image))
            {

                FileManager.Delete(_env.WebRootPath, "uploads/sliders", user.Image);

                user.Image = null;
            }

            _context.SaveChanges();

            return RedirectToAction("index","home");
        }

        [Authorize(Roles = "Member")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UserPasswordM passwordM)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (passwordM.Password != passwordM.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Sifre tekrari duzgun yazilmayib!");
                return View();
            }

            var result = await _userManager.ChangePasswordAsync(user, passwordM.OldPassword, passwordM.Password);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View();
            }

            return RedirectToAction("index", "home");
        }

        [Authorize(Roles = "Member")]
        public IActionResult Join(string userId, int page = 1)
        {
            if(!_context.AppUsers.Any(x=>x.Id == userId)) return PartialView("_NotFoundPartial");
            var query = _context.CourseJoins.Where(x => x.AppUserId == userId).AsQueryable();
            ViewBag.SelectedPage = 1;
            int totalJoins = query.Count();
            ViewBag.TotalPages = (int)Math.Ceiling(totalJoins / 5d);
            ViewBag.TotalJoins = totalJoins;
            ViewBag.UserId = userId;

            List<CourseJoin> joins = query.Include(x => x.Course).Take(5).ToList();

            return View(joins);
        }

        public IActionResult Pagenation(string userId, int page = 1)
        {
            if (!_context.AppUsers.Any(x => x.Id == userId)) return PartialView("_NotFoundPartial");
            var query = _context.CourseJoins.Where(x => x.AppUserId == userId).AsQueryable();
            ViewBag.SelectedPage = page;
            int totalJoins = query.Count();
            ViewBag.TotalPages = (int)Math.Ceiling(totalJoins / 5d);
            ViewBag.TotalJoins = totalJoins;
            ViewBag.UserId = userId;

            List<CourseJoin> joins = query.Include(x => x.Course).Skip((page-1)*5).Take(5).ToList();

            return PartialView("_JoinsPartial", joins);
        }
    }
}
