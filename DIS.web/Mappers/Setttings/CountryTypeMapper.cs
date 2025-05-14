using DIS.DataAccess.Entity.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Web.ViewModels;

namespace DIS.Web.Mappers.Setttings
{
    public class CountryTypeMapper
    {

        public QueryOptions<CountryType> PrepareQueryOptionForRepository(QueryOptions<CountryType> options, CountryTypeViewModel vm)
        {
            if (!string.IsNullOrEmpty(vm.name))
            {
                options.FilterBy = (x => x.name.Contains(vm.name));
            }
            if (options.SortColumnsName != null)
            {
                options.SortBy = new List<Func<CountryType, object>>();
                if (options.SortColumnName == "name")
                {
                    options.SortBy.Add((x => x.name));
                }
                else
                {
                    options.SortOrder = Infrastructure.Enumerations.SortOrder.DESC;
                    options.SortBy.Add((x => x.id));
                }
            }
            else
            {
                options.SortBy.Add((x => x.id));
            }
            return options;
        }
        public CountryType? MapViewModelToModel(CountryType? data, CountryTypeViewModel vm)
        {
            if (data != null)
            {
                data.name = vm.name;

            }
            return data;
        }
        public CountryTypeViewModel? MapModelToViewModel(CountryType data, CountryTypeViewModel vm)
        {
            if (data != null)
            {
                vm.id = data.id;
                vm.name = data.name;
            }
            return vm;
        }

        public PagedResult<CountryTypeViewModel> MapModelToListViewModel(PagedResult<CountryType> list)
        {
            PagedResult<CountryTypeViewModel> vmList = new PagedResult<CountryTypeViewModel>();
            foreach (var data in list.data)
            {
                CountryTypeViewModel vm = new CountryTypeViewModel();
                vm.id = data.id;
                vm.name = data.name;
                vmList.data.Add(vm);
            }
            vmList.total = vmList.data.Count;
            return vmList;
        }

    }
}
