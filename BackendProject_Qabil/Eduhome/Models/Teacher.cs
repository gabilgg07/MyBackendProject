using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 200)]
        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [Required]
        [StringLength(maximumLength:50)]
        public string FullName { get; set; }

        [StringLength(maximumLength: 50)]
        public string Position { get; set; }

        [StringLength(maximumLength: 600)]
        public string About { get; set; }

        [StringLength(maximumLength: 25)]
        public string Degree { get; set; }
        public int? Experience { get; set; }

        [StringLength(maximumLength: 100)]
        public string Email { get; set; }

        [StringLength(maximumLength: 25)]
        public string Phone { get; set; }

        [StringLength(maximumLength: 100)]
        public string Skype { get; set; }

        [StringLength(maximumLength: 100)]
        public string Fb { get; set; }

        [StringLength(maximumLength: 100)]
        public string Twit { get; set; }

        [StringLength(maximumLength: 100)]
        public string Pint { get; set; }

        [StringLength(maximumLength: 100)]
        public string Vimeo { get; set; }

        public List<TeacherEvent> TeacherEvents { get; set; }
        public List<TeacherSkill> TeacherSkills { get; set; }

    }
}
