using DIS.DataAccess.Entity;
using DIS.DataAccess.Entity.Settings;

namespace DIS.Web.ViewModels
{
    public class UserEntryViewModel
    {
        public int id { get; set; }
        public string name { get; set; } = string.Empty;
        public string username { get; set; } = string.Empty;

        public bool status { get; set; }
        public string token { get; set; } = string.Empty;
        public bool success { get; set; } = false;
        public List<string> messages { get; set; }
        public Role? role { get; set; }
        public UserEntryViewModel()
        {
            messages = new List<string>();
            role = new Role();
        }
    }
}
