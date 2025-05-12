using DIS.DataAccess.Entity.Settings;
using DIS.Web.ViewModels;

namespace DIS.Web.Mappers.Setttings
{
    public class DisasterCategoryMapper
    {
        public DisasterCategory? MapViewModelToModel(DisasterCategory? data,DisasterCategoryViewModel vm)
        {
            if(data != null)
            {
                data.name = vm.name;

            }
            return data;
        }
    }
}
