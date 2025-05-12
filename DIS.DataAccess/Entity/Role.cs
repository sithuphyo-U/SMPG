using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity
{
    [Table("role")]
    public class Role : BaseEntity
    {
        public string? name { get; set; }
        public string? description { get; set; }
       
    }
}
