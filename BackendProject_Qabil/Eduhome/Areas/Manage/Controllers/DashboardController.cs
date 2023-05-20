using Eduhome.Areas.Manage.ViewModels;
using Eduhome.DAL;
using Eduhome.Models;
using Microsoft.AspNetCore.Authorization;
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
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            DateTime dateNow = DateTime.Now;
            DateTime FirstDayOfMonth = new DateTime(dateNow.Year, dateNow.Month, 1);
            var mPrice = _context.CourseJoins.Include(x => x.Course).Where(x => x.IsAccepting == true && FirstDayOfMonth < x.JoinAt && x.JoinAt < dateNow).Average(x => x.Course.Fee);
            double monthlyPrice = (double)(mPrice is double ? mPrice:0);

            DateTime FirstDayOfYear = new DateTime(dateNow.Year, 1, 1);
            var yPrice = _context.CourseJoins.Include(x => x.Course).Where(x => x.IsAccepting == true && FirstDayOfYear < x.JoinAt && x.JoinAt < dateNow).Average(x => x.Course.Fee);
            double yearlyPrice = (double)(yPrice is double ? yPrice : 0);

            int usersCount = _context.AppUsers.Where(x => x.IsAdmin == false).Count();
            int joinedUsersCount = _context.AppUsers.Include(x=>x.CourseJoins).Where(x => x.IsAdmin == false && x.CourseJoins.Any(x=>x.IsAccepting==true)).Count();

            double joinedUsersPercent = 100 * (double)joinedUsersCount / usersCount;

            int pendingJoins = _context.CourseJoins.Where(x => x.IsAccepting == null).Count();

            List<Category> categories = _context.Categories.Include(x=>x.Courses).ThenInclude(x=>x.CourseJoins).ToList();

            DashBVM dashBVM = new DashBVM
            {
                RevenueThisMonth = monthlyPrice,
                RevenueThisYear = yearlyPrice,
                UserJoinAcceptingPercent = joinedUsersPercent,
                PendingJoinsCount = pendingJoins,
                Categories = categories
            };

            return View(dashBVM);
        }
    }
}
