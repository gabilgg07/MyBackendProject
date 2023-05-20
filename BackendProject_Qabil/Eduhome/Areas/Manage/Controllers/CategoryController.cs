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
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Category> categories = _context.Categories.ToList();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category ct)
        {
            if (!ModelState.IsValid) return View();

            Category category = new Category
            {
                Name = ct.Name,
                Desc = ct.Desc
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null) return PartialView("_NotFoundPartial");

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category ct)
        {
            if (!ModelState.IsValid) return View();

            Category category = _context.Categories.FirstOrDefault(x => x.Id == ct.Id);

            if (category == null) return PartialView("_NotFoundPartial");

            category.Name = ct.Name;
            category.Desc = ct.Desc;

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.FirstOrDefault(x => x.Id == id);

            if (category == null) return PartialView("_NotFoundPartial");

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category ct)
        {
            if (!ModelState.IsValid) return View();

            Category category = _context.Categories.FirstOrDefault(x => x.Id == ct.Id);

            _context.Categories.Remove(category);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
