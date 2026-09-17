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
    [Table("disasterInfo")]
    public class DisasterInfo :BaseEntity
    {
        public DateTime date { get; set; }
        public string? time {  get; set; }
        public string title { get; set; }
        public string? details { get; set; }
        public int remark { get; set; }
        [ForeignKey("remark")]
        public int? country_type_id { get; set; }
        [ForeignKey("country_type_id")]
        public virtual CountryType CountryType { get; set; }
        public int? country_id { get; set; }
        [ForeignKey("country_id")]
        public virtual Country Country { get; set; }
        public int? state_division_id { get; set; }
        [ForeignKey("state_division_id")]
        public virtual StateDivision StateDivision { get; set; }
        public int? township_id { get; set; }
        [ForeignKey("township_id")]
        public virtual Township Township { get; set; }
        public int? district_id { get; set; }
        [ForeignKey("district_id")]
        public virtual District District { get; set; }
        public int disasterCategory_id { get; set; }
        [ForeignKey("disasterCategory_id")]
        public virtual DisasterCategory DisasterCategory { get; set; }
        public int? subCategory_id { get; set; }
        [ForeignKey("subCategory_id")]
        public virtual DisasterSubCategory SubCategory { get; set; }
      
    }

}
