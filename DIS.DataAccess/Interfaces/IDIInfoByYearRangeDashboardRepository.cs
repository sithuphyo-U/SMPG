using DIS.DataAccess.Entity;
using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Interfaces
{
    public interface IDIInfoByYearRangeDashboardRepository 
    {
        List<DIInfoByYearRangeDashboard> GetInfoByYearRange(int year);
    }
}
