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
    public class country_countrytypeRepository : ReadWriteRepositoryBase<country_countrytype>, Icountry_countrytypeRepository
    {
        public country_countrytypeRepository(IDbContext context) : base(context)
        {
        }

       
        public List<country_countrytype> GetByCountryId(int country_id)
        {
            return CustomQuery().Where(x => x.country_id == country_id && x.deleted == false ).ToList();
        }

        //public List<country_countrytype> DeleteByCountryId(int country_id)
        //{
        //    var items = CustomQuery()
        //        .Where(x => x.country_id == country_id && x.deleted == false)
        //        .ToList();

        //    foreach (var item in items)
        //    {
        //        item.deleted = true;
        //    }

        //    SaveChanges();

        //    return items;
        //}

        public List<country_countrytype> DeletebyCountryId(int countryId)
        {
            var items = CustomQuery().Where(x => x.country_id == countryId && x.deleted == false).ToList();

            foreach (var item in items)
            {
                item.deleted = true;
            }

            SaveChanges();

            return items;
        }

        public country_countrytype GetByCountryTypeByCountryId(int? countryTypeId)
        {
            return CustomQuery().Where(x => x.country_type_id == countryTypeId && x.deleted == false).FirstOrDefault();
        }
    }
}
