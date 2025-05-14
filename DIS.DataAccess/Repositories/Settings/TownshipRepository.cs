using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories.Settings
{
    public class TownshipRepository : ReadWriteRepositoryBase<Township>, ITownshipRepository
    {
        public TownshipRepository(IDbContext context) : base(context)
        {
        }

        public Township? FindByName(string name)
        {
            return CustomQuery().Where(x => x.name == name).FirstOrDefault();
        }
    }
}
