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
    [Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SuperAdminController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {

            List<AppUser> admins = _userManager.Users.Where(x => x.IsAdmin && x.Id!=_userManager.GetUsersInRoleAsync("SuperAdmin").Result.FirstOrDefault().Id).ToList();

            return View(admins);
        }

        public IActionResult Create()
        {
            ViewBag.Roles = _roleManager.Roles.Where(x=>x.Name!="Member" && x.Name !="SuperAdmin");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AdminCreateM adminCM)
        {
            ViewBag.Roles = _roleManager.Roles.Where(x => x.Name != "Member" && x.Name != "SuperAdmin");
            if (_userManager.Users.Any(x => x.UserName == adminCM.UserName))
            {
                ModelState.AddModelError("UserName", "Bu username istifade edilir!");
                return View();
            }

            if (_userManager.Users.Any(x => x.Email == adminCM.Email))
            {
                ModelState.AddModelError("Email", "Bu email istifade edilir!");
                return View();
            }

            if (adminCM.Password != adminCM.ConfirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Sifre tekrari duzgun yazilmayib!");
                return View();
            }

            AppUser admin = new AppUser()
            {
                FullName = adminCM.FullName,
                Email = adminCM.Email,
                UserName = adminCM.UserName,
                IsAdmin = true
            };

            await _userManager.CreateAsync(admin, adminCM.Password);

            await _userManager.AddToRoleAsync(admin, adminCM.Role);

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Edit(string id)
        {
            ViewBag.Roles = _roleManager.Roles.Where(x => x.Name != "Member" && x.Name != "SuperAdmin");
            AppUser admin = await _userManager.FindByIdAsync(id);

            if (admin == null) return PartialView("_NotFoundPartial");

            AdminEditM adminEditM = new AdminEditM()
            {
                Id = admin.Id,
                FullName = admin.FullName,
                UserName = admin.UserName,
                Email = admin.Email,
                Role = _userManager.GetRolesAsync(admin).Result.FirstOrDefault()
            };

            return View(adminEditM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AdminEditM adminEditM)
        {
            ViewBag.Roles = _roleManager.Roles.Where(x => x.Name != "Member" && x.Name != "SuperAdmin");
            if (!ModelState.IsValid) return View();

            AppUser existAdmin = await _userManager.FindByIdAsync(adminEditM.Id);

            if (existAdmin == null) return PartialView("_NotFoundPartial");

            if (_userManager.Users.Any(x => x.UserName == adminEditM.UserName) && existAdmin.UserName != adminEditM.UserName)
            {
                ModelState.AddModelError("UserName", "Bu username istifade edilir!");
                return View(adminEditM);
            }
            if (_userManager.Users.Any(x => x.Email == adminEditM.Email) && existAdmin.Email != adminEditM.Email)
            {
                ModelState.AddModelError("Email", "Bu e-mail istifade edilir!");
                return View(adminEditM);
            }

            if (adminEditM.Password != null)
            {
                if (adminEditM.Password != adminEditM.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "Sifre tekrari duzgun yazilmayib!");
                    return View(adminEditM);
                }

                if (adminEditM.OldPassword == null)
                {
                    ModelState.AddModelError("OldPassword", "Sifrenizi qeyd edin!");
                    return View(adminEditM);
                }

                var result = await _userManager.ChangePasswordAsync(existAdmin, adminEditM.OldPassword, adminEditM.Password);

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                        return View();
                    }
                }

            }

            existAdmin.FullName = adminEditM.FullName;
            existAdmin.UserName = adminEditM.UserName;
            existAdmin.Email = adminEditM.Email;

            await _userManager.RemoveFromRoleAsync(existAdmin, _userManager.GetRolesAsync(existAdmin).Result.FirstOrDefault());

            await _userManager.AddToRoleAsync(existAdmin, adminEditM.Role);

            await _userManager.UpdateAsync(existAdmin);

            return RedirectToAction("index");
        }


        public async Task<IActionResult> Delete(string id)
        {
            AppUser admin = await _userManager.FindByIdAsync(id);

            if (admin == null) return PartialView("_NotFoundPartial");

            await _userManager.DeleteAsync(admin);

            return RedirectToAction("index");
        }

        public async Task<IActionResult> Set()
        {
            AppUser sadmin = await _userManager.FindByNameAsync(User.Identity.Name);

            AdminSetM superAdminM = new AdminSetM
            {
                FullName = sadmin.FullName,
                UserName = sadmin.UserName,
                Email = sadmin.Email
            };

            return View(superAdminM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Set(AdminSetM sAM)
        {
            if (!ModelState.IsValid) return View();

            AppUser sadmin = await _userManager.FindByNameAsync(User.Identity.Name);

            if (_userManager.Users.Any(x => x.UserName == sAM.UserName) && sadmin.UserName != sAM.UserName)
            {
                ModelState.AddModelError("UserName", "Bu username istifade edilir!");
                return View(sAM);
            }
            if (_userManager.Users.Any(x => x.Email == sAM.Email) && sadmin.Email != sAM.Email)
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

                var result = await _userManager.ChangePasswordAsync(sadmin, sAM.OldPassword, sAM.Password);

                if (!result.Succeeded)
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                        return View();
                    }
                }

            }

            sadmin.FullName = sAM.FullName;
            sadmin.UserName = sAM.UserName;
            sadmin.Email = sAM.Email;

            await _userManager.UpdateAsync(sadmin);

            return RedirectToAction("index", "dashboard");
        }
    }
}

