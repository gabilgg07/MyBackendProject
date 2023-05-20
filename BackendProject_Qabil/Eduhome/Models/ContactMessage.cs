using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public DateTime CreateAt { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        public string Subject { get; set; }

        [Required]
        [StringLength(maximumLength: 500)]
        public string Message { get; set; }

        [Required]
        [StringLength(maximumLength: 50)]
        public string FullName { get; set; }

        [Required]
        [StringLength(maximumLength: 100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
