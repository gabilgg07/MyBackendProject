using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class TeacherSkill
    {
        public int Id { get; set; }

        public int TeacherId { get; set; }

        [StringLength(maximumLength: 50)]
        public string Name { get; set; }

        public int Value { get; set; }

        public Teacher Teacher { get; set; }
    }
}
