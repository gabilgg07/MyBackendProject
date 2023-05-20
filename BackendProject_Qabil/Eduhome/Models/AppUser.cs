using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class AppUser:IdentityUser
    {
        [StringLength(maximumLength: 50)]
        public string FullName { get; set; }

        public bool IsAdmin { get; set; }

        [StringLength(maximumLength: 100)]
        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public List<CourseJoin> CourseJoins { get; set; }
        public List<CourseMessage> CourseMessages { get; set; }
        public List<EventMessage> EventMessages { get; set; }
        public List<ContactMessage> ContactMessages { get; set; }
    }
}
