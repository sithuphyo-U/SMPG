using DIS.DataAccess.Entity;
using DIS.DataAccess.Entity.Settings;
using System.ComponentModel.DataAnnotations.Schema;

namespace DIS.Web.ViewModels
{
    public class DisasterInfoViewModel
    {
        public int id { get; set; }
        public string? title { get; set; }
        public string? totalCountofNews { get; set; }
        public DateTime date { get; set; }
        public string? time { get; set; }
        
       public string? word { get; set; }
        public int? country_type_id { get; set; }
        public string? country_type_name { get; set; }
        public int? country_id { get; set; }
        public string? country_name { get; set; }
        public int? state_division_id { get; set; }   
        public string? state_division_name { get; set; }
        public int? township_id { get; set; }
        public string? township_name { get; set; }
        public int? district_id { get; set; }
        public string? district_name { get; set; }
        public int disaster_category_id { get; set; }
        public string? disasterCategory_name { get; set; }
        public int? subcategory_id { get; set; }
        public string? subCategory_name { get; set; }
        public string? from_date { get; set; }
        public string? to_date { get; set; }

        public DateTime? created_date { get; set; }
        public IFormFileCollection? file_list { get; set; } 
          
        public List<FileViewModel> Files_List { get; set; } = new List<FileViewModel>();

        public DisasterInfo? DisasterInfo { get; set; }           // <-- new
        public List<File_TB>? ExsitingFiles { get; set; }
        public List<FilteredFileViewModel> FilteredFiles { get; set; } = new List<FilteredFileViewModel>();
      


    }
}
