using DIS.DataAccess.Entity.Settings;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIS.Web.ViewModels
{
    public class DisasterInfoViewModel
    {
        public int id { get; set; }
        public string title { get; set; }
        public string details { get; set; }
        public DateTime? date { get; set; }
        public TimeOnly time { get; set; }
        public int remark { get; set; }
       
        public int country_type_id { get; set; }
        public string? country_type_name { get; set; }
        public int country_id { get; set; }
        public string? country_name { get; set; }
        public int state_division_id { get; set; }   
        public string? state_division_name { get; set; }
        public int township_id { get; set; }
        public string? township_name { get; set; }
        public int district_id { get; set; }
        public string? district_name { get; set; }
        public int disasterCategory_id { get; set; }
        public string? disasterCategory_name { get; set; }
        public int subCategory_id { get; set; }
        public string? subCategory_name { get; set; }


    }
}
