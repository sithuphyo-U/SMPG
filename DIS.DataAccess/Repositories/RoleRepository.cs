using DIS.DataAccess.Entity;
using DIS.DataAccess.Interfaces;
using DIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DIS.DataAccess.Repositories
{
    public class RoleRepository : ReadWriteRepositoryBase<Role>, IRoleRepository
    {
        public RoleRepository(IDbContext context) : base(context)
        {
        }
        public Role? FindByName(string name)
        {
            return CustomQuery().Where(x => x.name == name && x.deleted == false).FirstOrDefault();
        }
    }
}
