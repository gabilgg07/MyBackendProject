using Eduhome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.ViewModels
{
    public class HomeVM
    {
        public Setting Setting { get; set; }
        public List<Slider> Sliders { get; set; }
        public List<Notice> Notices { get; set; }
        public List<Feature> Features { get; set; }
        public List<Testimonial> Testimonials { get; set; }
    }
}
