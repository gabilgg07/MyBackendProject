using Eduhome.DAL;
using Eduhome.Models;
using Eduhome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Setting = _context.Settings.FirstOrDefault(),
                Sliders = _context.Sliders.OrderBy(x => x.Order).ToList(),
                Notices = _context.Notices.ToList(),
                Features = _context.Features.ToList(),
                Testimonials = _context.Testimonials.OrderBy(x => x.Order).ToList()
            };
            return View(homeVM);
        }

        public IActionResult About()
        {
            AboutVM aboutVM = new AboutVM
            {
                Setting = _context.Settings.FirstOrDefault(),
                Notices = _context.Notices.ToList(),
                Teachers = _context.Teachers.OrderByDescending(x=>x.Id).Take(4).ToList(),
                Testimonials = _context.Testimonials.OrderBy(x => x.Order).ToList()
            };

            return View(aboutVM);
        }

        public IActionResult Contact()
        {
            ContactVM contactVM = new ContactVM
            {
                Setting = _context.Settings.FirstOrDefault()
            };

            return View(contactVM);
        }

        public IActionResult SendContactMessage(ContactVM contactVM)
        {
            if(!ModelState.IsValid) return RedirectToAction("contact");

            ContactMessage contactMessage = new ContactMessage
            {
                AppUserId = contactVM.ContactMessage.AppUserId,
                FullName = contactVM.ContactMessage.FullName,
                Email = contactVM.ContactMessage.Email,
                Subject = contactVM.ContactMessage.Subject,
                Message = contactVM.ContactMessage.Message,
                CreateAt = DateTime.Now
            };

            _context.ContactMessages.Add(contactMessage);
            _context.SaveChanges();

            return RedirectToAction("contact");
        }
       
        public IActionResult CreatingSubs(string email)
        {
            if (_context.Subscribes.Any(x=>x.Email.ToLower()==email.ToLower()))
            {
                return Json(new { status = 400 });
            }

            Subscribe subscribe = new Subscribe
            {
                Email = email
            };

            _context.Subscribes.Add(subscribe);
            _context.SaveChanges();

            return Json(new { status = 200 });
        }
    }
}
