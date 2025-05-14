using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity
{
    [Table("Log")]
    public class Log: BaseEntity
    {
        public int user_id { get; set; }
        [ForeignKey("user_id")]
        public virtual User? user { get; set; }
        public int role_id { get; set; }
        [ForeignKey("role_id")]
        public virtual Role? role { get; set; }
        public string? program_code { get; set; }
        public string? action { get; set; }
        public DateTime timeaccessed { get; set; }
    }
}
