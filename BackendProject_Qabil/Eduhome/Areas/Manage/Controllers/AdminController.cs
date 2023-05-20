using Eduhome.Areas.Manage.ViewModels;
using Eduhome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "Admin,EditorAdmin")]
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public AdminController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IActionResult> Set()
        {
            AppUser admin = await _userManager.FindByNameAsync(User.Identity.Name);

            AdminSetM adminSetM = new AdminSetM
            {
                FullName = admin.FullName,
                UserName = admin.UserName,
                Email = admin.Email
            };

            return View(adminSetM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Set(AdminSetM sAM)
        {
            if (!ModelState.IsValid) return View();

            AppUser admin = await _userManager.FindByNameAsync(User.Identity.Name);

            if (_userManager.Users.Any(x => x.UserName == sAM.UserName) && admin.UserName != sAM.UserName)
            {
                ModelState.AddModelError("UserName", "Bu username istifade edilir!");
                return View(sAM);
            }
            if (_userManager.Users.Any(x => x.Email == sAM.Email) && admin.Email != sAM.Email)
            {
                ModelState.AddModelError("Email", "Bu e-mail istifade edilir!");
                return View(sAM);
            }

            if (sAM.Password != null)
            {
                if (sAM.Password != sAM.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Sifre tekrari duzgun yazilmayib!");
                    return View(sAM);
                }

                if (sAM.OldPassword == null)
                {
                    ModelState.AddModelError("OldPassword", "Sifrenizi qeyd edin!");
                    return View(sAM);
                }

                var result = await _userManager.ChangePasswordAsync(admin, sAM.OldPassword, sAM.Password);

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                        return View();
                    }
                }

            }

            admin.FullName = sAM.FullName;
            admin.UserName = sAM.UserName;
            admin.Email = sAM.Email;

            await _userManager.UpdateAsync(admin);

            return RedirectToAction("index", "dashboard");
        }
    }
}
