using DIS.DataAccess.Entity.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Infrastructure.Enumerations;
using DIS.Web.ViewModels;
using Microsoft.Data.SqlClient;

namespace DIS.Web.Mappers.Setttings
{
    public class DisasterCategoryMapper
    {
        public QueryOptions<DisasterCategory> PrepareQueryOptionForRepository(QueryOptions<DisasterCategory> options, DisasterCategoryViewModel vm)
        {
            if (!string.IsNullOrEmpty(vm.name))
            {
                options.FilterBy = (x => x.name.Contains(vm.name));
            }
            if(options.SortColumnsName != null)
            {
                options.SortBy = new List<Func<DisasterCategory, object>>();
                if(options.SortColumnName == "name")
                {
                    options.SortBy.Add((x => x.name));
                }
                else
                {
                    options.SortOrder = Infrastructure.Enumerations.SortOrder.DESC;
                    options.SortBy.Add((x=>x.id));
                }
            }
            else
            {
                options.SortBy.Add((x => x.id));
            }
            return options;
        }
        public DisasterCategory? MapViewModelToModel(DisasterCategory? data,DisasterCategoryViewModel vm)
        {
            if(data != null)
            {
                data.name = vm.name;

            }   
            return data;
        }
        public DisasterCategoryViewModel? MapModelToViewModel(DisasterCategory data,DisasterCategoryViewModel vm)
        {
            if(data != null)
            {
                vm.id = data.id;
                vm.name = data.name;
            }
            return vm;
        }
        
        public PagedResult<DisasterCategoryViewModel> MapModelToListViewModel(PagedResult<DisasterCategory> list)
        {
            PagedResult<DisasterCategoryViewModel> vmList = new PagedResult<DisasterCategoryViewModel>();
            foreach(var data in list.data)
            {
                DisasterCategoryViewModel vm = new DisasterCategoryViewModel();
                vm.id = data.id;
                vm.name = data.name;
                vmList.data.Add(vm);
            }
            vmList.total =vmList.data.Count;
            return vmList;
        }

    }
}
