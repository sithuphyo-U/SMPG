using DIS.DataAccess.Entity;
using DIS.DataAccess.Interfaces;
using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories
{
    public class File_TBRepository:ReadWriteRepositoryBase<File_TB>,IFile_TBRepository
    {
        public File_TBRepository(IDbContext context) : base(context)
        {
        }

        public List<File_TB> GetFilebyDisasterInfoId(int id)
        {
            return CustomQuery().Where(x => x.disastercategory_id == id && x.deleted == false).ToList();
        }           

    }
}
