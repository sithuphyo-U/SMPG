using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories.Settings
{
    public class DistrictRepository : ReadWriteRepositoryBase<District>, IDistrictRepository
    {
        public DistrictRepository(IDbContext context) : base(context)
        {
        }

        public District? FindByName(string name)
        {
            return CustomQuery().Where(x => x.name == name).FirstOrDefault();
        }

        public District GetCountryByDistrict(int id)
        {
            return CustomQuery().Where(x => x.id == id && x.deleted == false).FirstOrDefault();
        }

        public List<District>? GetDistrictByStateDivision(int id)
        {
           return CustomQuery().Where(x => x.state_division_id == id).ToList();
        }
    }
}
