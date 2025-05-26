using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using NPOI.SS.Formula.PTG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories.Settings
{
    public class CountryRepository : ReadWriteRepositoryBase<Country>, ICountryRepository
    {
        public CountryRepository(IDbContext context) : base(context)
        {
        }
       
        public Country? FindByName(string name)
        {
            return CustomQuery().Where(x => x.name == name && x.deleted == false).FirstOrDefault();

        }

        public List<Country>? GetCountrybyCountryType(int id)
        {
           return CustomQuery().Where(x => x.id == id && x.deleted == false).ToList();
        }

        public Country? GetCountrybyDivisionId(int id)
        {
           return CustomQuery().Where( x => x.id == id && x.deleted == false).FirstOrDefault();
        }

       
    }
}
