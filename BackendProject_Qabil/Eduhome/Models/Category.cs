using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [StringLength(maximumLength:50)]
        public string Name { get; set; }


        [StringLength(maximumLength: 500)]
        public string Desc { get; set; }

        public List<Course> Courses { get; set; }
    }
}
