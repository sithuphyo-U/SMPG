using DIS.DataAccess.Entity;
using DIS.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories
{
    public class HeaderRepository : ReadWriteRepositoryBase<Header>, IHeaderRepository
    {
        public HeaderRepository(IDbContext context) : base(context)
        {
        }
    }
}
