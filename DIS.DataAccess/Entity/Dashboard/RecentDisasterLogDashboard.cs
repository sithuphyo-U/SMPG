using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity.Dashboard
{
    public class RecentDisasterLogDashboard
    {
        public DateTime Date { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
    }
}
