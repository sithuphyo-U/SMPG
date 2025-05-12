using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity.Settings
{
    public class DisasterCategory : BaseEntity
    {
        public string? name { get; set; } = string.Empty;
    }
}
