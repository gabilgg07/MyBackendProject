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
    [Authorize(Roles = "SuperAdmin,Admin,EditorAdmin")]
    public class EvCategoryController : Controller
    {
        private readonly AppDbContext _context;

        public EvCategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<EventCategory> eventCategories = _context.EventCategories.ToList();
            return View(eventCategories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(EventCategory eC)
        {
            if (!ModelState.IsValid) return View();

            EventCategory eventCategory = new EventCategory
            {
                Name = eC.Name
            };

            _context.EventCategories.Add(eventCategory);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            EventCategory eventCategory = _context.EventCategories.FirstOrDefault(x => x.Id == id);

            if (eventCategory == null) return PartialView("_NotFoundPartial");

            return View(eventCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EventCategory eC)
        {
            if (!ModelState.IsValid) return View();

            EventCategory eventCategory = _context.EventCategories.FirstOrDefault(x => x.Id == eC.Id);

            if (eventCategory == null) return PartialView("_NotFoundPartial");

            eventCategory.Name = eC.Name;

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            EventCategory eventCategory = _context.EventCategories.FirstOrDefault(x => x.Id == id);

            if (eventCategory == null) return PartialView("_NotFoundPartial");

            return View(eventCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(EventCategory eC)
        {
            if (!ModelState.IsValid) return View();

            EventCategory eventCategory = _context.EventCategories.FirstOrDefault(x => x.Id == eC.Id);

            _context.EventCategories.Remove(eventCategory);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
