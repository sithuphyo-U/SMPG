using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories.Settings
{
    public class CountryTypeRepository : ReadWriteRepositoryBase<CountryType>, ICountryTypeRepository
    {
        public CountryTypeRepository(IDbContext context) : base(context)
        {
        }
        public CountryType? FindByName(string name)
        {
            return CustomQuery().Where(x => x.name == name && x.deleted == false).FirstOrDefault();
        }
    }
}
