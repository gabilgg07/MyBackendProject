using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class CourseJoin
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }

        public DateTime? JoinAt { get; set; }

        public bool? IsAccepting { get; set; }
    }
}
