using DIS.DataAccess.Entity.Settings;

namespace DIS.Web.ViewModels
{
    public class CountryViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? countryType_name { get; set; }
        //public int? country_type_id { get; set; }
        public List<string>? country_type_name { get; set; } = new();
        public List<int> CountryTypeListId { get; set; } = new();
       


        public CountryViewModel() { 
            CountryTypeListId = new List<int>();
            country_type_name = new List<string>();
        }

    }
}
