using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories.Settings
{
    public class StateDivisionRepository : ReadWriteRepositoryBase<StateDivision>, IStateDivisionRepository
    {
        public StateDivisionRepository(IDbContext context) : base(context)
        {
        }

        public StateDivision? FindByName(string name)
        {
            return CustomQuery().Where(x => x.name == name && x.deleted==false).FirstOrDefault();
        }

        public StateDivision GetCountryByStateDivision(int id)
        {
            return CustomQuery().Where(x => x.id == id && x.deleted==false).FirstOrDefault();
        }

        public List<StateDivision>? GetStateDivisionbyCountry(int id)
        {
            return CustomQuery().Where(x => x.country_id == id && x.deleted==false).ToList();
        }

       
    }
}
