using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Models
{
    public class Setting
    {
        public int Id { get; set; }

        [StringLength(maximumLength: 200)]
        public string TitleLogo { get; set; }

        [NotMapped]
        public IFormFile TitleImage { get; set; }

        [StringLength(maximumLength: 100)]
        public string HeaderLogo { get; set; }

        [NotMapped]
        public IFormFile HeaderImage { get; set; }

        [StringLength(maximumLength: 100)]
        public string ColorLogo { get; set; }

        [NotMapped]
        public IFormFile ColorImage { get; set; }

        [StringLength(maximumLength: 100)]
        public string FooterLogo { get; set; }

        [NotMapped]
        public IFormFile FooterImage { get; set; }

        [StringLength(maximumLength: 50)]
        public string ChooseTitle { get; set; }

        [StringLength(maximumLength: 1500)]
        public string ChooseText { get; set; }

        [StringLength(maximumLength: 30)]
        public string Phone1 { get; set; }

        [StringLength(maximumLength: 30)]
        public string Phone2 { get; set; }

        [StringLength(maximumLength: 100)]
        public string Address { get; set; }

        [StringLength(maximumLength: 100)]
        public string Email { get; set; }

        [StringLength(maximumLength: 100)]
        public string Site { get; set; }

        [StringLength(maximumLength: 100)]
        public string Facebook { get; set; }

        [StringLength(maximumLength: 100)]
        public string Pinteres { get; set; }

        [StringLength(maximumLength: 100)]
        public string Vimeo { get; set; }

        [StringLength(maximumLength: 100)]
        public string Twitter { get; set; }

        [StringLength(maximumLength: 100)]
        public string AboutTitle { get; set; }

        [StringLength(maximumLength: 1000)]
        public string AboutDesc { get; set; }

        [StringLength(maximumLength: 100)]
        public string AboutPic { get; set; }

        [NotMapped]
        public IFormFile AboutImage { get; set; }

        [StringLength(maximumLength: 100)]
        public string AboutVideo { get; set; }
    }
}
