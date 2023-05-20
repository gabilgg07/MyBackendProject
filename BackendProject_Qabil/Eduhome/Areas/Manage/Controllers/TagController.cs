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
    public class TagController : Controller
    {
        private readonly AppDbContext _context;

        public TagController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Tag> tags = _context.Tags.ToList();
            return View(tags);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Tag tg)
        {
            if (!ModelState.IsValid) return View();

            Tag tag = new Tag
            {
                Name = tg.Name
            };

            _context.Tags.Add(tag);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Tag tag = _context.Tags.FirstOrDefault(x => x.Id == id);

            if (tag == null) return PartialView("_NotFoundPartial");

            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Tag tg)
        {
            if (!ModelState.IsValid) return View();

            Tag tag = _context.Tags.FirstOrDefault(x => x.Id == tg.Id);

            if (tag == null) return PartialView("_NotFoundPartial");

            tag.Name = tg.Name;

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Tag tag = _context.Tags.FirstOrDefault(x => x.Id == id);

            if (tag == null) return PartialView("_NotFoundPartial");

            return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Tag tg)
        {
            if (!ModelState.IsValid) return View();

            Tag tag = _context.Tags.FirstOrDefault(x => x.Id == tg.Id);

            _context.Tags.Remove(tag);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
