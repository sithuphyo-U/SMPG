using DIS.DataAccess.Entity.Settings;

namespace DIS.Web.ViewModels
{
    public class DisasterSubCategoryViewModel
    {
        public int id { get; set; }
        public int category_id { get; set; } = 0;
        public string? category_name { get; set; }
        public string name { get; set; }
         
    }
}
