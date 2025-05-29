using DIS.DataAccess.Entity.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Interfaces.Dashboard
{
    public interface IDisasterCountRepository 
    {
        Task<IEnumerable<DisasterCountDashboard>> GetMonthlyDisasterCountsAsync(int year);
    }
}
