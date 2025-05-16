using DIS.DataAccess.Entity;
using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces;
using DIS.DataAccess.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories
{
    public class DisasterInfoRepository : ReadWriteRepositoryBase<DisasterInfo>,IDisasterInfoRepository
    {
        public DisasterInfoRepository(IDbContext context) : base(context)
        {
        }

       
        public DisasterInfo? FindByTitle(string title)
        {
            return CustomQuery().Where(x => x.title == title).FirstOrDefault();
        }
    }
}
