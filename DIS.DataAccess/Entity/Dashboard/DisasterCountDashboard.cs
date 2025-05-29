using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity.Dashboard
{
    public class DisasterCountDashboard
    {
        public int DisasterCategoryId { get; set; }
        public string CategoryName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int TotalRecords { get; set; }
    }
}
