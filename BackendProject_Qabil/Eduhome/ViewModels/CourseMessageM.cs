using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.ViewModels
{
    public class CourseMessageM
    {
        public int CourseId { get; set; }

        public string AppUserId { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Subject { get; set; }

        [StringLength(maximumLength: 500)]
        public string Message { get; set; }

        public DateTime CreateAt { get; set; }
    }
}
