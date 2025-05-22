namespace DIS.Web.ViewModels
{
    public class DistrictViewModel
    {

        public int id { get; set; }
        public string name { get; set; }
        public int country_id { get; set; }
        public string? country_name { get; set; }
        public int state_division_id {  get; set; }
        public string? state_division_name { get; set; }
        public string? countryType_name { get; set; }
        public List<string>? country_type_name { get; set; } = new();
        public List<int> CountryTypeListId { get; set; } = new();
        public DistrictViewModel()
        {
            CountryTypeListId = new List<int>();
            country_type_name = new List<string>();
        }

    }
}
