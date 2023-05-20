using Eduhome.DAL;
using Eduhome.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _context;

        public TeacherController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.FirstPage = 1;
            int totalTeachers = _context.Teachers.Count();
            ViewBag.TotalPages = (int)Math.Ceiling(totalTeachers / 4d);
            ViewBag.TotalTeachers = totalTeachers;
            List<Teacher> teachers = _context.Teachers
                .Include(t => t.TeacherSkills).Take(4).ToList();

            return View(teachers);
        }

        public IActionResult Pagenation(int page = 1)
        {
            int totalTeachers = _context.Teachers.Count();
            ViewBag.SelectedPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalTeachers / 4d);
            ViewBag.TotalTeachers = totalTeachers;
            List<Teacher> teachers = _context.Teachers
                .Include(t => t.TeacherSkills)
                .Skip((page - 1) * 4).Take(4).ToList();

            return PartialView("_TeachersPartial", teachers);
        }

        public IActionResult Detail(int id)
        {
            Teacher teacher = _context.Teachers.Include(t=>t.TeacherSkills).FirstOrDefault(x => x.Id == id);

            if (teacher == null) return PartialView("_NotFoundPartial");

            return View(teacher);
        }
    }
}
