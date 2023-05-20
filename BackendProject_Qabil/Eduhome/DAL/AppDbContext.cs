using Eduhome.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.DAL
{
    public class AppDbContext: IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseJoin> CourseJoins { get; set; }
        public DbSet<CourseMessage> CourseMessages { get; set; }
        public DbSet<CourseTag> CourseTags { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventMessage> EventMessages { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Notice> Notices { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<TeacherEvent> TeacherEvents { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<TeacherSkill> TeacherSkills { get; set; }
    }
}
