using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity.Dashboard
{
   
    public class CategoryCardDashboard 
    {
        public int disasterCategory_id { get; set; }
        public int count { get; set; }
    }
}
