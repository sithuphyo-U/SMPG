using DIS.DataAccess.Entity.Settings;
using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Interfaces.Settings
{
    public interface IStateDivisionRepository : IReadWriteRepositoryBase<StateDivision>
    {
        StateDivision? FindByName(string name);
         List<StateDivision>? GetStateDivisionbyCountry(int id);
    }
}
