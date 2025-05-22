using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity.Settings
{
    [Table("township")]
    public class Township:BaseEntity
    {
        public string? name { get; set; }
        public int country_id { get; set; }
        [ForeignKey("country_id")]
        public virtual Country Country { get; set; }
        //public int country_type_id { get; set; }
        //[ForeignKey("country_type_id")]
        //public virtual CountryType CountryType { get; set; }
        public int state_division_id { get; set; }
        [ForeignKey("state_division_id")]
        public virtual StateDivision StateDivision { get; set; }
        public int district_id { get; set; }
        [ForeignKey("district_id")]
        public virtual District District { get; set; }
    }
}
