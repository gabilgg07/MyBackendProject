using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class Course
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 200)]
        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [StringLength(maximumLength: 50)]
        public string Name { get; set; }

        [StringLength(maximumLength: 1500)]
        public string About { get; set; }

        [StringLength(maximumLength: 1500)]
        public string Apply { get; set; }

        [StringLength(maximumLength: 1500)]
        public string Certification { get; set; }

        public DateTime? Starts { get; set; }

        public int? Duration { get; set; }

        [Column(TypeName = "Tinyint")]
        public byte? ClassDuration { get; set; }

        [StringLength(maximumLength: 50)]
        public string Language { get; set; }

        public int? StudentsLimit { get; set; }

        public double? Fee { get; set; }
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [NotMapped]
        public List<int> TagIds { get; set; }

        public List<CourseTag> CourseTags { get; set; }
        public List<CourseJoin> CourseJoins { get; set; }
        public List<CourseMessage> CourseMessages { get; set; }
    }
}
