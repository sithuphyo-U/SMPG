namespace DIS.Web.ViewModels
{
    public class UserViewModel
    {
        public int id { get; set; }
        public string? name { get; set; } = string.Empty;
        public string? username { get; set; } = string.Empty;
        public string? password { get; set; } = string.Empty;
        public bool? status { get; set; }
        public int? role_id { get; set; } = 0;
        public string? role_name { get; set; }
    }
}
