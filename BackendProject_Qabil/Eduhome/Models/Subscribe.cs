using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class Subscribe
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 200)]
        public string Email { get; set; }

        public DateTime? Date { get; set; }
    }
}
