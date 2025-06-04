using DIS.DataAccess.Entity;
using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Web.ViewModels;
using Newtonsoft.Json;

namespace DIS.Web.Mappers.Setttings
{
    public class HeaderMapper
    {
        public QueryOptions<Header> PrepareQueryOptionForRepository(QueryOptions<Header> options, HeaderViewModel vm)
        {


            return options;
        }

        public Header? MapViewModelToModel(Header? data, HeaderViewModel vm)
        {
            if (data != null)
            {

                data.header_name = vm.header_name;
               



            }
            return data;
        }
        public List<HeaderViewModel> MapModelToListViewModel(List<Header> list)
        {
            List<HeaderViewModel> vmList = new List<HeaderViewModel>();
            foreach (var data in list)
            {
                HeaderViewModel vm = new HeaderViewModel();
                vm.id = data.id;
                vm.header_name = data.header_name;
              

                vmList.Add(vm);
            }
            
            return vmList;
        }

    }
}
