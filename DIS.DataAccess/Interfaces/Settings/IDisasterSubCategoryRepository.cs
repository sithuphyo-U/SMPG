
﻿using DIS.DataAccess.Entity.Settings;
using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Interfaces.Settings
{
    public interface IDisasterSubCategoryRepository : IReadWriteRepositoryBase<DisasterSubCategory>
    {
        DisasterSubCategory? FindByName(string name);
        List<DisasterSubCategory>? GetSubCategorybyCategory(int id);
        List<DisasterSubCategory>? GetByName(string name);
    }
}