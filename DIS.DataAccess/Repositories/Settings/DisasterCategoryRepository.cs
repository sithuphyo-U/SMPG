using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DMS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories.Settings
{
    public class DisasterCategoryRepository : ReadWriteRepositoryBase<DisasterCategory>, IDisasterCategoryRepository
    {
        public DisasterCategoryRepository(IDbContext context) : base(context)
        {
        }
    }
}
