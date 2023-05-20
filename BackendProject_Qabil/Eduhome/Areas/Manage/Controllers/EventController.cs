using Eduhome.DAL;
using Eduhome.Helpers;
using Eduhome.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin,EditorAdmin")]
    public class EventController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public EventController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            List<Event> events = _context.Events.Include(x=>x.EventCategory).Include(x=>x.EventMessages).ToList();

            return View(events);
        }

        public IActionResult Detail(int id)
        {
            Event ev = _context.Events
                .Include(x => x.EventCategory)
                .Include(x => x.TeacherEvents).ThenInclude(x => x.Teacher)
                .FirstOrDefault(x => x.Id == id);

            if (ev == null) return PartialView("_NotFoundPartial");

            return View(ev);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.EventCategories.ToList();
            ViewBag.Teachers = _context.Teachers.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Event ev)
        {
            ViewBag.Categories = _context.EventCategories.ToList();
            ViewBag.Teachers = _context.Teachers.ToList();

            if (!ModelState.IsValid) return View(ev);

            if (!_context.EventCategories.Any(x => x.Id == ev.EventCategoryId))
            {
                ModelState.AddModelError("EventCategoryId", "Have not Category like this name!");
                return View();
            }

            Event even = new Event
            {
                Name = ev.Name,
                Desc = ev.Desc,
                Address = ev.Address,
                StartDate = ev.StartDate,
                StartHour = ev.StartHour,
                EndHour = ev.EndHour,
                EventCategoryId = ev.EventCategoryId
            };

            even.TeacherEvents = new List<TeacherEvent>();

            if (ev.TeacherIds != null)
            {
                foreach (var teacherId in ev.TeacherIds)
                {
                    TeacherEvent teacherEvent = new TeacherEvent
                    {
                        TeacherId = teacherId
                    };

                    even.TeacherEvents.Add(teacherEvent);
                }
            }

            if (ev.ImageFile != null)
            {
                if (ev.ImageFile.ContentType != "image/jpeg" && ev.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "You can choose file only .jpg, .jpeg or .png format!");
                    return View();
                }

                if (ev.ImageFile.Length > 5242880)
                {
                    ModelState.AddModelError("ImageFile", "You can choose file only maximum 5Mb !");
                    return View();
                }

                even.Image = FileManager.Save(_env.WebRootPath, "uploads/events", ev.ImageFile);
            }

            _context.Events.Add(even);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/events", even.Image);
            }


            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Event even = _context.Events
                .Include(x => x.EventCategory)
                .Include(x => x.TeacherEvents).ThenInclude(x => x.Teacher)
                .FirstOrDefault(x => x.Id == id);

            if (even == null) return PartialView("_NotFoundPartial");

            even.TeacherIds = even.TeacherEvents.Select(x => x.TeacherId).ToList();

            ViewBag.Categories = _context.EventCategories.ToList();
            ViewBag.Teachers = _context.Teachers.ToList();

            return View(even);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Event ev)
        {

            ViewBag.Categories = _context.EventCategories.ToList();
            ViewBag.Teachers = _context.Teachers.ToList();

            if (!ModelState.IsValid) return View();

            Event even = _context.Events.Include(x => x.TeacherEvents).FirstOrDefault(x => x.Id == ev.Id);

            if (even == null) return PartialView("_NotFoundPartial");

            if (!_context.EventCategories.Any(x => x.Id == ev.EventCategoryId))
            {
                ModelState.AddModelError("EventCategoryId", "Have not Category like this name!");
                return View();
            }

            if (ev.ImageFile != null)
            {
                if (ev.ImageFile.ContentType != "image/jpeg" && ev.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Yalniz .jpg , .jpeg ve ya .png formatda fayl sece bilersiz!");
                    return View();
                }
                if (ev.ImageFile.Length > 5242880)
                {
                    ModelState.AddModelError("ImageFile", "Maksimum uzunlugu 5Mb olan fayl sece bilersiz!");
                    return View();
                }
            }

            even.Name = ev.Name;
            even.Desc = ev.Desc;
            even.Address = ev.Address;
            even.StartDate = ev.StartDate;
            even.StartHour = ev.StartHour;
            even.EndHour = ev.EndHour;
            even.EventCategoryId = ev.EventCategoryId;

            if (ev.ImageFile != null)
            {
                string newFileName = FileManager.Save(_env.WebRootPath, "uploads/events", ev.ImageFile);

                if (!string.IsNullOrWhiteSpace(even.Image))
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/events", even.Image);
                }

                even.Image = newFileName;
            }
            else if (string.IsNullOrWhiteSpace(ev.Image) && !string.IsNullOrWhiteSpace(even.Image))
            {

                FileManager.Delete(_env.WebRootPath, "uploads/events", even.Image);

                even.Image = null;
            }

            if (ev.TeacherIds != null)
            {
                even.TeacherEvents.Clear();
                List<TeacherEvent> teacherEvents = new List<TeacherEvent>();
                foreach (var teacherId in ev.TeacherIds)
                {
                    TeacherEvent teacherEvent = new TeacherEvent()
                    {
                        TeacherId = teacherId
                    };
                    teacherEvents.Add(teacherEvent);
                }
                even.TeacherEvents = teacherEvents;
            }
            else
            {
                even.TeacherEvents.Clear();
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {

                FileManager.Delete(_env.WebRootPath, "uploads/events", even.Image);


                return PartialView("_NotFoundPartial");
            }

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Event even = _context.Events
                .Include(x => x.EventCategory)
                .Include(x => x.TeacherEvents).ThenInclude(x => x.Teacher)
                .FirstOrDefault(x => x.Id == id);

            if (even == null) return PartialView("_NotFoundPartial");

            even.TeacherIds = even.TeacherEvents.Select(x => x.TeacherId).ToList();
            ViewBag.Categories = _context.EventCategories.ToList();
            ViewBag.Teachers = _context.Teachers.ToList();

            return View(even);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(Event ev)
        {
            if (!ModelState.IsValid) return View();

            Event even = _context.Events.FirstOrDefault(x => x.Id == ev.Id);

            if (even == null) return PartialView("_NotFoundPartial");

            if (!string.IsNullOrWhiteSpace(even.Image))
            {
                FileManager.Delete(_env.WebRootPath, "uploads/events", even.Image);
            }

            _context.Events.Remove(even);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Message(int eventId)
        {
            List<EventMessage> eventMessages = _context.EventMessages.Include(x => x.AppUser).Where(x => x.EventId == eventId).ToList();

            return View(eventMessages);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult DeleteMessage(int id)
        {
            EventMessage eventMessage = _context.EventMessages.FirstOrDefault(x => x.Id == id);

            if(eventMessage == null) return PartialView("_NotFoundPartial");


            return View(eventMessage);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMessage(EventMessage eM)
        {
            EventMessage eventMessage = _context.EventMessages.FirstOrDefault(x => x.Id == eM.Id);

            if (eventMessage == null) return PartialView("_NotFoundPartial");

            _context.EventMessages.Remove(eventMessage);
            _context.SaveChanges();

            return RedirectToAction("message", new { eventId = eventMessage.EventId });
        }
    }
}
