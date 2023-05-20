using Eduhome.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eduhome.Areas.Manage.ViewModels
{
    public class DashBVM
    {
        public double RevenueThisMonth { get; set; }
        public double RevenueThisYear { get; set; }
        public double UserJoinAcceptingPercent { get; set; }
        public int PendingJoinsCount { get; set; }

        public List<Category> Categories { get; set; }
    }
}
