namespace DIS.Web.ViewModels
{
    public class LogViewModel
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public string user_name { get; set; } = string.Empty;
        public string controller { get; set; } = string.Empty;
        public string table { get; set; } = string.Empty;
        public string action { get; set; } = string.Empty;
        public string url { get; set; } = string.Empty;
        public string date { get; set; } = string.Empty;
        public string? ip { get; set; } = string.Empty;
    }
}

