using DIS.DataAccess.Entity;
using DIS.DataAccess.Entity.Settings;
using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Interfaces
{
    public interface IUserRepository : IReadWriteRepositoryBase<User>
    {
        User? FindByName(string name);
        User? FindByUserName(string userName);
    }
}
