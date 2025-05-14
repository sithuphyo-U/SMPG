using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories.Settings
{
    public class DisasterSubCategoryRepository : ReadWriteRepositoryBase<DisasterSubCategory>, IDisasterSubCategoryRepository
    {
        public DisasterSubCategoryRepository(IDbContext context) : base(context)
        {
        }

        public DisasterSubCategory? FindByName(string name)
        {
            return CustomQuery().Where(x => x.name  == name).FirstOrDefault();
        }
    }
}
