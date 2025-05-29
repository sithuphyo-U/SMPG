using DIS.DataAccess.Entity.Dashboard;
using DIS.DataAccess.Interfaces.Dashboard;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories.Dashboard
{
    public class DisasterCountRepository : IDisasterCountRepository
    {
        private readonly DbContext _context;

        public DisasterCountRepository(IDbContext context)
        {
            _context = (DbContext)context;
        }
        public async Task<IEnumerable<DisasterCountDashboard>> GetMonthlyDisasterCountsAsync(int year)
        {
            var yearParam = new SqlParameter("@Year", year);

            return await _context.Set<DisasterCountDashboard>().FromSqlRaw("EXEC GetDisasterCountsByMonth @Year", yearParam).ToListAsync();
         
            
        }
    }
}
