using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.ViewModels
{
    public class UserLoginM
    {
        [Required]
        [StringLength(maximumLength: 25)]
        public string Username { get; set; }

        [Required]
        [StringLength(maximumLength: 25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
