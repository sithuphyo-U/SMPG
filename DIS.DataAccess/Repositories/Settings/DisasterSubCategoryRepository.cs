using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
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
            return CustomQuery().Where(x => x.name == name && x.deleted == false).FirstOrDefault();
        }

        public List<DisasterSubCategory>? GetByName(string name)
        {
            return CustomQuery().Where(x => x.name == name && x.deleted == false).ToList();
        }

        public List<DisasterSubCategory>? GetSubCategorybyCategory(int id)
        {
            return CustomQuery().Where(x => x.disaster_category_id == id && x.deleted == false).ToList();
        }
    }
}
