using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity.Settings
{
    [Table("country_type")]
    public class CountryType :BaseEntity
    {
        public string? name {  get; set; } = string.Empty;

    }
}
