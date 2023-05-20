using Eduhome.DAL;
using Eduhome.Helpers;
using Eduhome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles ="SuperAdmin,Admin,EditorAdmin")]
    public class SettingController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SettingController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            Setting setting = _context.Settings.FirstOrDefault();

            return View(setting);
        }

        public IActionResult Update()
        {
            Setting setting = _context.Settings.FirstOrDefault();
            return View(setting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Setting set)
        {
            if (!ModelState.IsValid) return View();

            Setting setting = _context.Settings.FirstOrDefault();

            if (set.TitleImage != null)
            {
                if (set.TitleImage.ContentType != "image/jpeg" && set.TitleImage.ContentType != "image/png")
                {
                    ModelState.AddModelError("TitleImage", "You can choose file only .jpg, .jpeg or .png format!");
                    return View(set);
                }

                if (set.TitleImage.Length > 5242880)
                {
                    ModelState.AddModelError("TitleImage", "You can choose file only maximum 5Mb !");
                    return View(set);
                }

                string newFileName = FileManager.Save(_env.WebRootPath, "uploads/setting", set.TitleImage);

                if (!string.IsNullOrWhiteSpace(setting.TitleLogo))
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/setting", setting.TitleLogo);
                }

                setting.TitleLogo = newFileName;
            }

            if (set.HeaderImage != null)
            {
                if (set.HeaderImage.ContentType != "image/jpeg" && set.HeaderImage.ContentType != "image/png")
                {
                    ModelState.AddModelError("HeaderImage", "You can choose file only .jpg, .jpeg or .png format!");
                    return View(set);
                }

                if (set.HeaderImage.Length > 5242880)
                {
                    ModelState.AddModelError("HeaderImage", "You can choose file only maximum 5Mb !");
                    return View(set);
                }

                string newFileName = FileManager.Save(_env.WebRootPath, "uploads/setting", set.HeaderImage);

                if (!string.IsNullOrWhiteSpace(setting.HeaderLogo))
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/setting", setting.HeaderLogo);
                }

                setting.HeaderLogo = newFileName;
            }

            if (set.ColorImage != null)
            {
                if (set.ColorImage.ContentType != "image/jpeg" && set.ColorImage.ContentType != "image/png")
                {
                    ModelState.AddModelError("ColorImage", "You can choose file only .jpg, .jpeg or .png format!");
                    return View(set);
                }

                if (set.ColorImage.Length > 5242880)
                {
                    ModelState.AddModelError("ColorImage", "You can choose file only maximum 5Mb !");
                    return View(set);
                }

                string newFileName = FileManager.Save(_env.WebRootPath, "uploads/setting", set.ColorImage);

                if (!string.IsNullOrWhiteSpace(setting.ColorLogo))
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/setting", setting.ColorLogo);
                }

                setting.ColorLogo = newFileName;
            }

            if (set.FooterImage != null)
            {
                if (set.FooterImage.ContentType != "image/jpeg" && set.FooterImage.ContentType != "image/png")
                {
                    ModelState.AddModelError("FooterImage", "You can choose file only .jpg, .jpeg or .png format!");
                    return View(set);
                }

                if (set.FooterImage.Length > 5242880)
                {
                    ModelState.AddModelError("FooterImage", "You can choose file only maximum 5Mb !");
                    return View(set);
                }

                string newFileName = FileManager.Save(_env.WebRootPath, "uploads/setting", set.FooterImage);

                if (!string.IsNullOrWhiteSpace(setting.FooterLogo))
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/setting", setting.FooterLogo);
                }

                setting.FooterLogo = newFileName;
            }

            if (set.AboutImage != null)
            {
                if (set.AboutImage.ContentType != "image/jpeg" && set.AboutImage.ContentType != "image/png")
                {
                    ModelState.AddModelError("AboutImage", "You can choose file only .jpg, .jpeg or .png format!");
                    return View(set);
                }

                if (set.AboutImage.Length > 5242880)
                {
                    ModelState.AddModelError("AboutImage", "You can choose file only maximum 5Mb !");
                    return View(set);
                }

                string newFileName = FileManager.Save(_env.WebRootPath, "uploads/setting", set.AboutImage);

                if (!string.IsNullOrWhiteSpace(setting.AboutPic))
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/setting", setting.AboutPic);
                }

                setting.AboutPic = newFileName;
            }

            
            setting.ChooseTitle = set.ChooseTitle;
            setting.AboutTitle = set.AboutTitle;
            setting.ChooseText = set.ChooseText;
            setting.AboutDesc = set.AboutDesc;
            setting.Address = set.Address;
            setting.Phone1 = set.Phone1;
            setting.Phone2 = set.Phone2;
            setting.Email = set.Email;
            setting.Site = set.Site;
            setting.AboutVideo = set.AboutVideo;
            setting.Facebook = set.Facebook;
            setting.Pinteres = set.Pinteres;
            setting.Twitter = set.Twitter;
            setting.Vimeo = set.Vimeo;

           

            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
