using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DIS.DataAccess.Entity;
using DIS.Infrastructure.Common;

namespace DIS.DataAccess.Interfaces
{
    public interface IRoleRepository : IReadWriteRepositoryBase<Role>
    {
        Role? FindByName(string name);
    }
}
