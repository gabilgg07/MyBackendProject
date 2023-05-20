using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class Slider
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 200)]
        public string Title { get; set; }

        [StringLength(maximumLength: 200)]
        public string Subtitle { get; set; }

        [StringLength(maximumLength: 500)]
        public string Desc { get; set; }

        [StringLength(maximumLength: 200)]
        public string Image { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [Required]
        public int Order { get; set; }
    }
}
