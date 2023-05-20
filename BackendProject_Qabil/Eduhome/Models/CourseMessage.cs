using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class CourseMessage
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        [StringLength(maximumLength: 100)]
        public string Subject { get; set; }

        [StringLength(maximumLength: 500)]
        public string Message { get; set; }

        public DateTime? Date { get; set; }
    }
}
