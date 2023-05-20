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
    public class NoticeController : Controller
    {
        private readonly AppDbContext _context;

        public NoticeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Notice> notices = _context.Notices.ToList();

            return View(notices);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Notice nt)
        {
            if (!ModelState.IsValid) return View();

            Notice notice = new Notice
            {
                Text = nt.Text,
                CreatAt = nt.CreatAt
            };

            _context.Notices.Add(notice);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Notice notice = _context.Notices.FirstOrDefault(x => x.Id == id);

            if (notice == null) return PartialView("_NotFoundPartial");

            return View(notice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Notice nt)
        {
            if (!ModelState.IsValid) return View();

            Notice notice = _context.Notices.FirstOrDefault(x => x.Id == nt.Id);

            if (notice == null) return PartialView("_NotFoundPartial");

            notice.Text = nt.Text;
            notice.CreatAt = nt.CreatAt;

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Notice notice = _context.Notices.FirstOrDefault(x => x.Id == id);

            if (notice == null) return PartialView("_NotFoundPartial");

            return View(notice);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Notice nt)
        {
            if (!ModelState.IsValid) return View();

            Notice notice = _context.Notices.FirstOrDefault(x => x.Id == nt.Id);

            _context.Notices.Remove(notice);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
