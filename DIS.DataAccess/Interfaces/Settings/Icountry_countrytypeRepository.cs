using DIS.DataAccess.Entity.Settings;
using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Interfaces.Settings
{
    public interface Icountry_countrytypeRepository :IReadWriteRepositoryBase<country_countrytype>
    {
        List<country_countrytype> GetByCountryId(int countryId);
       List<country_countrytype> DeletebyCountryId(int countryId);
        List<country_countrytype> GetByCountryTypeByCountryId(int? countryId);
    }
}
