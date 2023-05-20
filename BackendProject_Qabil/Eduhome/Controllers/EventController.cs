using Eduhome.DAL;
using Eduhome.Models;
using Eduhome.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _context;

        public EventController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? categoryId, string name)
        {
            var query = _context.Events.AsQueryable();
            if (categoryId != null)
            {
                query = query.Where(x => x.EventCategoryId == categoryId);
            }
            if (name != null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }

            ViewBag.SelectedPage = 1;
            int totalEvents = query.Count();
            ViewBag.TotalPages = (int)Math.Ceiling(totalEvents / 4d);
            ViewBag.TotalEvents = totalEvents;
            ViewBag.CategoryId = categoryId;
            ViewBag.Categories = _context.EventCategories.Include(x=>x.Events).ToList();
            ViewBag.Name = name;
            List<Event> events = query
                .Include(x => x.EventCategory)
                .Include(x => x.TeacherEvents).ThenInclude(x => x.Teacher)
                .Take(4).ToList();

            return View(events);
        }

        public IActionResult Pagenation(int? categoryId, string name, int page = 1)
        {
            var query = _context.Events.AsQueryable();
            if (categoryId != null)
            {
                query = query.Where(x => x.EventCategoryId == categoryId);
            }
            if (name != null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            int totalEvents = query.Count();
            ViewBag.SelectedPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalEvents / 4d); ViewBag.TotalEvents = totalEvents;
            ViewBag.CategoryId = categoryId;
            ViewBag.Categories = _context.EventCategories.Include(x => x.Events).ToList();
            ViewBag.Name = name;
            List<Event> events = query
                .Include(x => x.EventCategory)
                .Include(x => x.TeacherEvents).ThenInclude(x => x.Teacher)
                .Skip((page - 1) * 4).Take(4).ToList();

            return PartialView("_EventsPartial", events);
        }

        public IActionResult Detail(int id)
        {
            Event even = _context.Events
               .Include(x => x.EventCategory)
               .Include(x => x.TeacherEvents).ThenInclude(x => x.Teacher)
               .FirstOrDefault(x => x.Id == id);
            if (even == null) return PartialView("_NotFoundPartial");
            ViewBag.Categories = _context.EventCategories.Include(x => x.Events).ToList();

            EventDetailM detailM = new EventDetailM
            {
                Event = even
            };

            return View(detailM);
        }

        public IActionResult SendEventMessage(EventDetailM detailM)
        {
            if(!ModelState.IsValid) return RedirectToAction("detail", new { id = detailM.EventMessage.EventId });

            EventMessage eventMessage = new EventMessage
            {
                EventId = detailM.EventMessage.EventId,
                AppUserId = detailM.EventMessage.AppUserId,
                Name = detailM.EventMessage.Name,
                Email = detailM.EventMessage.Email,
                Subject = detailM.EventMessage.Subject,
                Message = detailM.EventMessage.Message,
                Date = DateTime.Now
            };

            _context.EventMessages.Add(eventMessage);
            _context.SaveChanges();

            return RedirectToAction("detail", new { id = detailM.EventMessage.EventId });
        }
    }
}
