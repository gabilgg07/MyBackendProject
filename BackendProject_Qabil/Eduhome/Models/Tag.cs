using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class Tag
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 25)]
        public string Name { get; set; }

        public List<CourseTag> CourseTags { get; set; }
    }
}
