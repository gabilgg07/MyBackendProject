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
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;

        public CourseController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? categoryId, string name, int? tagId)
        {
            var query = _context.Courses.AsQueryable();
            if (categoryId!=null)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }
            if (name != null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            if (tagId != null)
            {
                query = query.Where(x => x.CourseTags.Any(x=>x.TagId == tagId));
            }
            ViewBag.SelectedPage = 1;
            int totalCourses = query.Count();
            ViewBag.TotalPages = (int)Math.Ceiling(totalCourses / 3d);
            ViewBag.TotalCourses = totalCourses;
            ViewBag.CategoryId = categoryId;
            ViewBag.Name = name;
            ViewBag.TagId = tagId;
            List<Course> courses = query
                .Include(c => c.Category)
                .Include(c => c.CourseTags).ThenInclude(ct => ct.Tag)
                .Take(3).ToList();

            return View(courses);
        }

        public IActionResult Pagenation(int? categoryId, int? tagId , string name, int page = 1)
        {
            var query = _context.Courses.AsQueryable(); 
            if (categoryId != null)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }
            if (tagId != null)
            {
                query = query.Where(x => x.CourseTags.Any(x => x.TagId == tagId));
            }
            if (name != null)
            {
                query = query.Where(x => x.Name.ToLower().Contains(name.ToLower()));
            }
            int totalCourses = query.Count();
            ViewBag.SelectedPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCourses / 3d);
            ViewBag.TotalCourses = totalCourses;
            ViewBag.CategoryId = categoryId;
            ViewBag.Name = name;
            ViewBag.TagId = tagId;
            List<Course> courses = query
                 .Include(c => c.Category)
                 .Include(c => c.CourseTags).ThenInclude(ct => ct.Tag)
                 .Skip((page - 1) * 3).Take(3).ToList();

            return PartialView("_CoursesPartial", courses);
        }

        public IActionResult Search(string name, int? categoryId)
        {
            if (name == null) return Json(new { status = 404 });

            var query = _context.Courses.AsQueryable();

            if (categoryId != null)
            {
              query = query.Where(x => x.CategoryId == categoryId);
            }

            List<Course> courses = query.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();

            if (courses == null) return Json(new { status = 404 });

            return PartialView("_CoursesSearchPartial", courses);
        }

        public IActionResult Detail(int id)
        {
            Course course = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.CourseTags).ThenInclude(ct => ct.Tag)
                .FirstOrDefault(c => c.Id == id);
            if(course == null) return PartialView("_NotFoundPartial");
            ViewBag.Categories = _context.Categories.Include(x=>x.Courses).ToList();

            CourseDetailM courseDetailM = new CourseDetailM
            {
                Course = course
            };

            var userName = User.Identity.Name;
            ViewBag.User = _context.AppUsers.Include(x=>x.CourseJoins).FirstOrDefault(x => x.UserName == userName);

            return View(courseDetailM);
        }

       
        public IActionResult SendCourseMessage(CourseDetailM detailM)
        {
            CourseMessage courseMessage = new CourseMessage
            {
                CourseId = detailM.CourseMessage.CourseId,
                AppUserId = detailM.CourseMessage.AppUserId,
                Subject = detailM.CourseMessage.Subject,
                Message = detailM.CourseMessage.Message,
                Date = DateTime.Now
            };

            _context.CourseMessages.Add(courseMessage);
            _context.SaveChanges();

            return RedirectToAction("detail", new { id = detailM.CourseMessage.CourseId });
        }

        public IActionResult Join(int courseId, string userId)
        {
            if(!_context.Courses.Any(x=>x.Id == courseId)) return PartialView("_NotFoundPartial");

            if(!_context.AppUsers.Any(x=>x.Id == userId)) return PartialView("_NotFoundPartial");

            CourseJoin courseJoin = new CourseJoin
            {
                CourseId = courseId,
                AppUserId = userId,
                JoinAt = DateTime.Now
            };

            _context.CourseJoins.Add(courseJoin);
            _context.SaveChanges();

            return RedirectToAction("index");
        }
    }
}
