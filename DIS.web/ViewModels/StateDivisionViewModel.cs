using DIS.DataAccess.Entity.Settings;

namespace DIS.Web.ViewModels
{
    public class StateDivisionViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string? countryType_name { get; set; }

        public List<string>? country_type_name { get; set; } = new();
        public List<int> CountryTypeListId { get; set; } = new();
        public int country_id { get; set; }
        public string country_name { get; set; }
        public int? country_type_id { get; set; }
        public List<country_countrytype> cc_type { get; set; } = new();
        public StateDivisionViewModel()
        {
            CountryTypeListId = new List<int>();
            country_type_name = new List<string>();
        }
    }
}
