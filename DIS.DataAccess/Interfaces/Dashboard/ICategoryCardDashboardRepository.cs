using DIS.DataAccess.Entity.Dashboard;
using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Interfaces.Dashboard
{
    public interface ICategoryCardDashboardRepository
    {

        Task<List<CategoryCardDashboard>> GetDisasterCountAsync();
    }
}
