using DIS.DataAccess.Entity;
using DIS.Infrastructure.Common;
using DIS.DataAccess.Entity.Settings;
using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity
{
    [Table("users")]
    public class User : BaseEntity
    {
        public string username { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;

        public int? role_id { get; set; }
        [ForeignKey("role_id")]
        public virtual Role? role { get; set; }
        public bool status { get; set; }

    }
}
