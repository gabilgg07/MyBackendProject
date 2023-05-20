using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class Notice
    {
        public int Id { get; set; }

        public DateTime CreatAt { get; set; }

        [StringLength(maximumLength: 400)]
        public string Text { get; set; }
    }
}
