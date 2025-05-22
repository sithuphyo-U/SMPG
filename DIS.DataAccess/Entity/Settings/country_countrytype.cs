using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity.Settings
{
    [Table("country_countrytype")]
    public class country_countrytype :BaseEntity
    {
        public int country_id {  get; set; }
        [ForeignKey("country_id")]
        public virtual Country? Country {  get; set; }
        public int country_type_id { get; set; }
        [ForeignKey("country_type_id")]
        public virtual CountryType? CountryType { get; set; }

    }
}
