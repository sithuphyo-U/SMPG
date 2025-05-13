namespace DIS.Web.ViewModels
{
    public class ProgramCodeViewModel
    {
        public int? id {  get; set; }
        public string? program_name { get; set; }
        public string? program_code { get; set; }
        public int? parent_id { get; set; }
        public string? url { get; set; }
        public string? icon { get; set; }
        public string? permission { get; set; }
        public List<PermissionViewModel>? permissions { get; set; }
        public List<object> children { get; set; }
        public ProgramCodeViewModel()
        {
            permissions = new List<PermissionViewModel>();
            children = new List<object>();
        }

    }
}
