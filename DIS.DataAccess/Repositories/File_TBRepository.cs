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

    }
}
