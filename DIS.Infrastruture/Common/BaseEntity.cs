using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.Infrastructure.Common
{
    public abstract class BaseEntity
    {
        [Key]
        public int id { get; set; }
        public bool deleted { get; set; }
        public DateTime? created_date { get; set; }
        public int? created_by { get; set; }
        public DateTime? modified_date { get; set; }
        public int? modified_by { get; set; }
    }
}
