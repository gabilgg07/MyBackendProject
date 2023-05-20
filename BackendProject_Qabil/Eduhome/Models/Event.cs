using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class Event
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 70)]
        public string Name { get; set; }

        [StringLength(maximumLength: 200)]
        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        public int EventCategoryId { get; set; }

        public DateTime? StartDate { get; set; }

        public TimeSpan? StartHour { get; set; }
        public TimeSpan? EndHour { get; set; }

        [StringLength(maximumLength: 150)]
        public string Address { get; set; }

        [StringLength(maximumLength: 1500)]
        public string Desc { get; set; }

        [NotMapped]
        public List<int> TeacherIds { get; set; }

        public EventCategory EventCategory { get; set; }

        public List<TeacherEvent> TeacherEvents { get; set; }
        public List<EventMessage> EventMessages { get; set; }
    }
}
