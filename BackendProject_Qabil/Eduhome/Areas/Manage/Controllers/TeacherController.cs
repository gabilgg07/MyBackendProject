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
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public TeacherController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Teacher> teachers = _context.Teachers.ToList();
            return View(teachers);
        }

        public IActionResult Detail(int id)
        {
            Teacher teacher = _context.Teachers.Include(x => x.TeacherSkills).FirstOrDefault(x => x.Id == id);

            if(teacher==null) return PartialView("_NotFoundPartial");

            return View(teacher);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Teacher thr)
        {
            if (!ModelState.IsValid) return View(thr);

            Teacher teacher = new Teacher
            {
                FullName = thr.FullName,
                Position = thr.Position,
                About = thr.About,
                Degree = thr.Degree,
                Experience = thr.Experience,
                Email = thr.Email,
                Phone = thr.Phone,
                Skype = thr.Skype,
                Fb = thr.Fb,
                Pint = thr.Pint,
                Vimeo = thr.Vimeo,
                Twit = thr.Twit
            };

            if (thr.TeacherSkills!=null)
            {
                teacher.TeacherSkills = thr.TeacherSkills;
            }

            if (thr.ImageFile != null)
            {
                if (thr.ImageFile.ContentType != "image/jpeg" && thr.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "You can choose file only .jpg, .jpeg or .png format!");
                    return View();
                }

                if (thr.ImageFile.Length > 5242880)
                {
                    ModelState.AddModelError("ImageFile", "You can choose file only maximum 5Mb !");
                    return View();
                }

                teacher.Image = FileManager.Save(_env.WebRootPath, "uploads/teachers", thr.ImageFile);
            }

            _context.Teachers.Add(teacher);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                FileManager.Delete(_env.WebRootPath, "uploads/teachers", teacher.Image);
            }


            return RedirectToAction("index");
        }

        public IActionResult Edit(int id)
        {
            Teacher teacher = _context.Teachers.Include(x=>x.TeacherSkills).FirstOrDefault(x => x.Id == id);

            if (teacher == null) return PartialView("_NotFoundPartial");

            return View(teacher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Teacher thr)
        {
            if (!ModelState.IsValid) return View(thr);

            Teacher teacher = _context.Teachers.Include(x=>x.TeacherSkills).FirstOrDefault(x => x.Id == thr.Id);

            if (teacher == null) return PartialView("_NotFoundPartial");

            teacher.FullName = thr.FullName;
            teacher.Position = thr.Position;
            teacher.About = thr.About;
            teacher.Degree = thr.Degree;
            teacher.Experience = thr.Experience;
            teacher.Email = thr.Email;
            teacher.Phone = thr.Phone;
            teacher.Skype = thr.Skype;
            teacher.Fb = thr.Fb;
            teacher.Pint = thr.Pint;
            teacher.Vimeo = thr.Vimeo;
            teacher.Twit = thr.Twit;


            if (thr.TeacherSkills != null)
            {
                _context.TeacherSkills.RemoveRange(teacher.TeacherSkills);
                teacher.TeacherSkills = thr.TeacherSkills;
            }


            if (thr.ImageFile != null)
            {
                if (thr.ImageFile.ContentType != "image/jpeg" && thr.ImageFile.ContentType != "image/png")
                {
                    ModelState.AddModelError("ImageFile", "Yalniz .jpg , .jpeg ve ya .png formatda fayl sece bilersiz!");
                    return View();
                }
                if (thr.ImageFile.Length > 5242880)
                {
                    ModelState.AddModelError("ImageFile", "Maksimum uzunlugu 5Mb olan fayl sece bilersiz!");
                    return View();
                }

                string newFileName = FileManager.Save(_env.WebRootPath, "uploads/teachers", thr.ImageFile);

                if (!string.IsNullOrWhiteSpace(teacher.Image))
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/teachers", teacher.Image);
                }

                teacher.Image = newFileName;
            }
            else if (string.IsNullOrWhiteSpace(thr.Image) && !string.IsNullOrWhiteSpace(teacher.Image))
            {

                FileManager.Delete(_env.WebRootPath, "uploads/teachers", teacher.Image);

                teacher.Image = null;
            }

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult Delete(int id)
        {
            Teacher teacher = _context.Teachers.Include(x=>x.TeacherSkills).FirstOrDefault(x => x.Id == id);

            if (teacher == null) return PartialView("_NotFoundPartial");

            return View(teacher);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Teacher thr)
        {
            if (!ModelState.IsValid) return View();

            Teacher teacher = _context.Teachers.FirstOrDefault(x => x.Id == thr.Id);

            if (teacher == null) return PartialView("_NotFoundPartial");

            if (!string.IsNullOrWhiteSpace(teacher.Image))
            {
                FileManager.Delete(_env.WebRootPath, "uploads/teachers", teacher.Image);
            }

            _context.Teachers.Remove(teacher);
            _context.SaveChanges();

            return View();
        }
    }
}
