using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Repositories.Settings
{

    public class LabelRepository : ReadWriteRepositoryBase<Label>, ILabelRepository
    {
        public LabelRepository(IDbContext context) : base(context)
        {
        }

    }
}
