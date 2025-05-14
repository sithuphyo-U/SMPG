namespace DIS.Web.ViewModels
{
    public class TownshipViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int country_type_id { get; set; }
        public string? country_type_name { get; set; }
        public int country_id { get; set; }
        public string? country_name { get; set; }
        public int state_division_id { get; set; }
        public string? state_division_name { get; set; }
        public int district_id { get; set; }
        public string? district_name { get; set; }
    }
}
