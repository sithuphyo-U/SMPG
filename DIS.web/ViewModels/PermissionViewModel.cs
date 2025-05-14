namespace DIS.Web.ViewModels
{
    public class PermissionViewModel
    {

        public int id { get; set; }
        public int? program_code_id { get; set; }
        public string? program_name { get; set; }
        public int? role_id { get; set; }
        public string? role_name { get; set; }
        public bool? read { get; set; }
        public bool? write { get; set; }
        public bool? delete { get; set; }
    }
}
