using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity.Settings
{
    [Table("countries")]
    public class Country : BaseEntity
    {
        public string? name { get; set; }
        public int country_type_id { get; set; }
        [ForeignKey("country_type_id")]
        public virtual CountryType CountryType { get; set; }
    }
}
