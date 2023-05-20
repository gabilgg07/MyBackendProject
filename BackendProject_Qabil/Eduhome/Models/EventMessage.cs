using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class EventMessage
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public Event Event { get; set; }

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public DateTime Date { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Subject { get; set; }

        [StringLength(maximumLength: 500)]
        public string Message { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

    }
}
