using DIS.DataAccess.Entity;
using DIS.Infrastructure.Enumerations;
using DIS.Infrastructure.Utilities;
using DIS.Web.ViewModels;

namespace DIS.Web.Mappers
{
    public class RoleMapper
    {
        public QueryOptions<Role> PrepareQueryOptionForRepository(QueryOptions<Role> queryOption, RoleViewModel vm)
        {

            if (!string.IsNullOrEmpty(vm.name))
            {
                queryOption.FilterBy = (x => x.name.Contains(vm.name));
            }

            if (queryOption.SortColumnName != null)
            {
                queryOption.SortBy = new List<Func<Role, object>>();
                if (queryOption.SortColumnName == "name")
                {
                    queryOption.SortBy.Add((x => x.name));
                }
                else
                {
                    queryOption.SortOrder = SortOrder.DESC;
                    queryOption.SortBy.Add((x => x.id));
                }
            }
            else
            {
                queryOption.SortBy.Add((x => x.id));
            }
            return queryOption;
        }
        public PagedResult<RoleViewModel> MapModelToListViewMode(PagedResult<Role> list)
        {
            PagedResult<RoleViewModel> vmList = new PagedResult<RoleViewModel>();
            foreach (var data in list.data)
            {
                RoleViewModel vm = new RoleViewModel();
                vm.id = data.id;
                vm.name = data.name;
                vm.description = data.description;
                vmList.data.Add(vm);
            }
            //vmList.total = vmList.data.Count;
            vmList.total = list.total;
            vmList.success = list.success;
            vmList.messages = new List<string>(list.messages);
            return vmList;
        }
        public Role? MapViewModelToModel(Role? data, RoleViewModel vm)
        {
            if (data != null)
            {
                data.name = vm.name;
                data.description = vm.description;
                
            }
            return data;
        }
        public RoleViewModel MapModelToViewModel(Role? data, RoleViewModel vm)
        {
            if (data != null)
            {
                vm.id = data.id;
                vm.name = data.name;
                vm.description = data.description;
              
            }
            return vm;
        }
    }
}
