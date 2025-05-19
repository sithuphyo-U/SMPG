using DIS.DataAccess.Entity.Settings;
using DIS.Infrastructure.Enumerations;
using DIS.Infrastructure.Utilities;
using DIS.Infrastruture.Utilities;
using DIS.Web.ViewModels;

namespace DIS.Web.Mappers.Setttings
{
    public class CountryMapper
    {

        public QueryOptions<Country> PrepareQueryOptionForRepository(QueryOptions<Country> options, CountryViewModel vm)
        {
            if (vm.country_type_id > 0)
            {
                options.FilterBy = (x => x.country_type_id == vm.country_type_id);
            }
            if (!string.IsNullOrEmpty(vm.name))
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, (x => x.name.Contains(vm.name)));
            }

            if (options.SortColumnName != null)
            {
                options.SortBy = new List<Func<Country, object>>();
                if (options.SortColumnName == "name")
                {
                    options.SortBy.Add((x => x.name));
                }
                else if (options.SortColumnName == "country_type_name")
                {
                    options.SortBy.Add((x => x.CountryType.name));
                }
                else
                        {
                    options.SortOrder = SortOrder.DESC;
                    options.SortBy.Add((x => x.id));
                }
            }
            else
            {
                options.SortBy.Add((x => x.id));
            }
            return options;
        }

      
        public Country? MapViewModelToModel(Country? data, CountryViewModel vm)
        {
            if (data != null)
            {

                data.name = vm.name;
                if (vm.country_type_id > 0)
                {
                    data.country_type_id = vm.country_type_id;
                }


            }
            return data;
        }
        public CountryViewModel? MapModelToViewModel(Country data, CountryViewModel vm)
        {
            if (data != null)
            {
                vm.id = data.id;
                vm.name = data.name;
                if (data.CountryType != null)
                {
                    vm.country_type_id = data.country_type_id;
                    vm.country_type_name = data.CountryType.name;
                }
            }
            return vm;
        }

        public PagedResult<CountryViewModel> MapModelToListViewModel(PagedResult<Country> list)
        {
            PagedResult<CountryViewModel> vmList = new PagedResult<CountryViewModel>();
            foreach (var data in list.data)
            {
                CountryViewModel vm = new CountryViewModel();
                vm.id = data.id;
                vm.name = data.name;
                if (data.CountryType != null)
                {
                    vm.country_type_name = data.CountryType.name;
                }
                vmList.data.Add(vm);
            }
            vmList.total = vmList.data.Count;
            return vmList;
        }

    }
}
