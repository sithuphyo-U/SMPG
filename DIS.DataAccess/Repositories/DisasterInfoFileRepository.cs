using DIS.DataAccess.Entity;
using DIS.DataAccess.Interfaces;
using DIS.Infrastructure.Common;
using DIS.Infrastructure.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories
{
    public class DisasterInfoFileRepository : ReadWriteRepositoryBase<File_TB>,IDisasterInfoFileRepository
    {
        public DisasterInfoFileRepository(IDbContext context) : base(context)
        {
        }

        public List<File_TB> GetDataById(int id)
        {
            return CustomQuery().Where(x => x.disastercategory_id == id && x.deleted == false).ToList();

        }


      
    }
}
