using Eduhome.DAL;
using Eduhome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SubscribeController : Controller
    {
        private readonly AppDbContext _context;

        public SubscribeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Subscribe> subscribes = _context.Subscribes.ToList();
            return View(subscribes);
        }

        public IActionResult Edit(int id)
        {
            Subscribe subscribe = _context.Subscribes.FirstOrDefault(x => x.Id == id);

            if (subscribe == null) return PartialView("_NotFoundPartial");

            return View(subscribe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Subscribe sub)
        {
            if (!ModelState.IsValid) return View();

            Subscribe subscribe = _context.Subscribes.FirstOrDefault(x => x.Id == sub.Id);

            if (subscribe == null) return PartialView("_NotFoundPartial");

            subscribe.Email = sub.Email;

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Subscribe subscribe = _context.Subscribes.FirstOrDefault(x => x.Id == id);

            if (subscribe == null) return PartialView("_NotFoundPartial");

            return View(subscribe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Subscribe sub)
        {
            if (!ModelState.IsValid) return View();

            Subscribe subscribe = _context.Subscribes.FirstOrDefault(x => x.Id == sub.Id);

            _context.Subscribes.Remove(subscribe);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
