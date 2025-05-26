using DIS.DataAccess.Entity;
using DIS.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories.Settings
{
    public class DIInfoByYearRangeRepository :  IDIInfoByYearRangeDashboardRepository
    {
        private readonly IDbContext _context;
        public DIInfoByYearRangeRepository(IDbContext context) 
        {
            _context = context;
        }

        public List<DIInfoByYearRangeDashboard> GetInfoByYearRange(int year)
        {
            // return RawSQL<DIInfoByYearRangeDashboard>("exec GetDisasterDataByYearRange {0}", new object[] { year }).ToList();
            return _context.Set<DIInfoByYearRangeDashboard>()
                    .FromSqlRaw("EXEC GetDisasterDataByYearRange {0}", year)
                    .ToList();
        }
    }
}
