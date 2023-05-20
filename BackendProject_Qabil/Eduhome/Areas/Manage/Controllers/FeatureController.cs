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
    public class FeatureController : Controller
    {
        private readonly AppDbContext _context;

        public FeatureController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            List<Feature> features = _context.Features.ToList();

            return View(features);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Feature ft)
        {
            if (!ModelState.IsValid) return View();

            Feature feature = new Feature
            {
                Title = ft.Title,
                Desc = ft.Desc
            };

            _context.Features.Add(feature);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Feature feature = _context.Features.FirstOrDefault(x => x.Id == id);

            if (feature == null) return PartialView("_NotFoundPartial");

            return View(feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Feature ft)
        {
            Feature feature = _context.Features.FirstOrDefault(x => x.Id == ft.Id);

            if (feature == null) return PartialView("_NotFoundPartial");

            feature.Title = ft.Title;
            feature.Desc = ft.Desc;

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Feature feature = _context.Features.FirstOrDefault(x => x.Id == id);

            if (feature == null) return PartialView("_NotFoundPartial");

            return View(feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Feature ft)
        {
            if (!ModelState.IsValid) return View();

            Feature feature = _context.Features.FirstOrDefault(x => x.Id == ft.Id);

            _context.Features.Remove(feature);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
