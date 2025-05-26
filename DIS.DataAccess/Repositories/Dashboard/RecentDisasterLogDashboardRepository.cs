using DIS.DataAccess.Entity.Dashboard;
using DIS.DataAccess.Interfaces.Dashboard;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories.Dashboard
{
    public class RecentDisasterLogDashboardRepository : IRecentDisasterLogDashboardRepository
    {
     
        private readonly DbContext _dbContext;
        public RecentDisasterLogDashboardRepository(IDbContext dbContext)
        {
            _dbContext = (DbContext)dbContext;
        }
        public async Task<List<RecentDisasterLogDashboard>> GetRecentDisasterLogsAsync()
        {
            return await _dbContext.Set<RecentDisasterLogDashboard>() .FromSqlRaw("EXEC GetLatestDisastersByCategory").ToListAsync();
        }
    }
}
