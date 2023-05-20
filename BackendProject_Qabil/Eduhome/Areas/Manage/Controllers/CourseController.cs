using Eduhome.DAL;
using Eduhome.Helpers;
using Eduhome.Models;
using Eduhome.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Areas.Manage.Controllers
{
    [Area("Manage")]
    [Authorize(Roles = "SuperAdmin,Admin,EditorAdmin")]
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IEmailService _emailService;

        public CourseController(AppDbContext context, IWebHostEnvironment env, IEmailService emailService)
        {
            _context = context;
            _env = env;
            _emailService = emailService;
        }
        public IActionResult Index()
        {
            List<Course> courses = _context.Courses.Include(x=>x.CourseMessages).Include(x=>x.CourseJoins).ToList();

            return View(courses);
        }

        public IActionResult Detail(int id)
        {
            Course course = _context.Courses
                .Include(c=>c.Category)
                .Include(c=>c.CourseTags).ThenInclude(ct=>ct.Tag)
                .FirstOrDefault(x => x.Id == id);

            if(course == null) return PartialView("_NotFoundPartial");

            return View(course);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Course crs)
        {
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (!ModelState.IsValid) return View(crs);

            if (!_context.Categories.Any(x => x.Id == crs.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Have not Category like this name!");
                return View();
            }

            Course course = new Course
            {
                Name = crs.Name,
                About = crs.About,
                Apply = crs.Apply,
                Certification = crs.Certification,
                Starts = crs.Starts,
                Duration = crs.Duration,
                ClassDuration = crs.ClassDuration,
                Language = crs.Language,
                StudentsLimit = crs.StudentsLimit,
                Fee = crs.Fee,
                CategoryId = crs.CategoryId
            };

            course.CourseTags = new List<CourseTag>();

            if (crs.TagIds != null)
            {
                foreach (var tagId in crs.TagIds)
                {
                    CourseTag courseTag = new CourseTag
                    {
                        TagId = tagId
                    };

                    course.CourseTags.Add(courseTag);
                }
            }

            if (crs.ImageFile != null)
            {
                if (crs.ImageFile.ContentType != "image/jpeg" && crs.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "You can choose file only .jpg, .jpeg or .png format!");
                    return View();
                }

                if (crs.ImageFile.Length > 5242880)
                {
                    ModelState.AddModelError("ImageFile", "You can choose file only maximum 5Mb !");
                    return View();
                }

                course.Image = FileManager.Save(_env.WebRootPath, "uploads/courses", crs.ImageFile);
            }

            _context.Courses.Add(course);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/courses", course.Image);
            }


            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Course course = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.CourseTags).ThenInclude(ct => ct.Tag)
                .FirstOrDefault(x => x.Id == id);

            if (course == null) return PartialView("_NotFoundPartial");

            course.TagIds = course.CourseTags.Select(x => x.TagId).ToList();

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(Course crs)
        {

            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            if (!ModelState.IsValid) return View();

            Course course = _context.Courses.Include(x=>x.CourseTags).FirstOrDefault(x => x.Id == crs.Id);

            if (course == null) return PartialView("_NotFoundPartial");

            if (!_context.Categories.Any(x => x.Id == crs.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Have not Category like this name!");
                return View();
            }

            if (crs.ImageFile != null)
            {
                if (crs.ImageFile.ContentType != "image/jpeg" && crs.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Yalniz .jpg , .jpeg ve ya .png formatda fayl sece bilersiz!");
                    return View();
                }
                if (crs.ImageFile.Length > 5242880)
                {
                    ModelState.AddModelError("ImageFile", "Maksimum uzunlugu 5Mb olan fayl sece bilersiz!");
                    return View();
                }
            }

            course.Name = crs.Name;
            course.About = crs.About;
            course.Apply = crs.Apply;
            course.Certification = crs.Certification;
            course.Starts = crs.Starts;
            course.Duration = crs.Duration;
            course.ClassDuration = crs.ClassDuration;
            course.Language = crs.Language;
            course.StudentsLimit = crs.StudentsLimit;
            course.Fee = crs.Fee;
            course.CategoryId = crs.CategoryId;

            if (crs.ImageFile != null)
            { 
                string newFileName = FileManager.Save(_env.WebRootPath, "uploads/courses", crs.ImageFile);

                if (!string.IsNullOrWhiteSpace(course.Image))
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/courses", course.Image);
                }

                course.Image = newFileName;
            }
            else if (string.IsNullOrWhiteSpace(crs.Image) && !string.IsNullOrWhiteSpace(course.Image))
            {

                FileManager.Delete(_env.WebRootPath, "uploads/courses", course.Image);

                course.Image = null;
            }

            if (crs.TagIds != null)
            {
                course.CourseTags.Clear();
                List<CourseTag> courseTags = new List<CourseTag>();
                foreach (var tagId in crs.TagIds)
                {
                    CourseTag courseTag = new CourseTag()
                    {
                        TagId = tagId
                    };
                    courseTags.Add(courseTag);
                }
                course.CourseTags = courseTags;
            }
            else
            {
                course.CourseTags.Clear();
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                
              FileManager.Delete(_env.WebRootPath, "uploads/courses", course.Image);
               

                return PartialView("_NotFoundPartial");
            }

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Course course = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.CourseTags).ThenInclude(ct => ct.Tag)
                .FirstOrDefault(x => x.Id == id);

            if (course == null) return PartialView("_NotFoundPartial");

            course.TagIds = course.CourseTags.Select(x => x.TagId).ToList();
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Tags = _context.Tags.ToList();

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Course crs)
        {
            if (!ModelState.IsValid) return View();

            Course course = _context.Courses.FirstOrDefault(x => x.Id == crs.Id);

            if (course == null) return PartialView("_NotFoundPartial");

            if (!string.IsNullOrWhiteSpace(course.Image))
            {
                FileManager.Delete(_env.WebRootPath, "uploads/courses", course.Image);
            }

            _context.Courses.Remove(course);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Message(int courseId)
        {
            List<CourseMessage> courseMessages = _context.CourseMessages.Include(x => x.AppUser).Where(x => x.CourseId == courseId).ToList();

            return View(courseMessages);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult Join(int courseId)
        {
            List<CourseJoin> courseJoins = _context.CourseJoins.Include(x => x.AppUser).Where(x => x.CourseId == courseId).ToList();

            return View(courseJoins);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult JoinAccept(int courseId, int id)
        {
            CourseJoin join = _context.CourseJoins.Include(x=>x.AppUser).FirstOrDefault(x => x.Id == id && x.CourseId == courseId);

            if (join == null) return PartialView("_NotFoundPartial");

            ViewBag.CourseId = courseId;

            join.IsAccepting = true;

            _context.SaveChanges();

            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/templates/JoinAccept.html"))
            {
                body = reader.ReadToEnd();
            }

            _emailService.Send(join.AppUser.Email, "Join accepted!", body);

            return RedirectToAction("join", new { courseId = join.CourseId });
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        public IActionResult JoinReject(int courseId, int id)
        {
            CourseJoin join = _context.CourseJoins.Include(x => x.AppUser).FirstOrDefault(x => x.Id == id && x.CourseId == courseId);

            if (join == null) return PartialView("_NotFoundPartial");

            ViewBag.CourseId = courseId;

            join.IsAccepting = false;

            _context.SaveChanges();

            string body = string.Empty;

            using(StreamReader reader= new StreamReader("wwwroot/templates/JoinReject.html"))
            {
                body = reader.ReadToEnd();
            }

            _emailService.Send(join.AppUser.Email, "Join rejected!", body);

            return RedirectToAction("join", new { courseId = join.CourseId });
        }
    }
}
