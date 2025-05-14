using DIS.DataAccess.Entity;
using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories
{
    public class UserRepository : ReadWriteRepositoryBase<User>, IUserRepository
    {
        public UserRepository(IDbContext context) : base(context)
        {
        }

        public User? FindByName(string name)
        {
            return CustomQuery().Where(x => x.username == name && x.deleted == false).FirstOrDefault();
        }

        public User? FindByUserName(string userName)
        {
            return CustomQuery().Where(x => x.username == userName && x.deleted == false).FirstOrDefault();
        }
    }
}
