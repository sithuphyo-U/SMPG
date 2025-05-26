using DIS.DataAccess.Entity;
using DIS.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories
{
    public class LogRepository : ReadWriteRepositoryBase<Log>, ILogRepository
    {
        public LogRepository(IDbContext context) : base(context)
        {
        }
    }
}
