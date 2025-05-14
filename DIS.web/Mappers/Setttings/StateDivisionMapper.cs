using DIS.DataAccess.Entity.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Web.ViewModels;

namespace DIS.Web.Mappers.Setttings
{
    public class StateDivisionMapper
    {
        public QueryOptions<StateDivision> PrepareQueryOptionForRepository(QueryOptions<StateDivision> options, StateDivisionViewModel vm)
        {
            if (!string.IsNullOrEmpty(vm.name))
            {
                options.FilterBy = (x => x.name.Contains(vm.name));
            }
            if (options.SortColumnsName != null)
            {
                options.SortBy = new List<Func<StateDivision, object>>();
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
        public StateDivision? MapViewModelToModel(StateDivision? data, StateDivisionViewModel vm)
        {
            if (data != null)
            {

                data.name = vm.name;
                if (vm.country_type_id > 0)
                {
                    data.country_type_id = vm.country_type_id;
                }
                if (vm.country_id > 0)
                {
                    data.country_id = vm.country_id;
                }


            }
            return data;
        }
        public StateDivisionViewModel? MapModelToViewModel(StateDivision data, StateDivisionViewModel vm)
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
                if (data.Country != null)
                {
                    vm.country_id = data.country_id;
                    vm.country_name = data.Country.name;
                }
            }
            return vm;
        }

        public PagedResult<StateDivisionViewModel> MapModelToListViewModel(PagedResult<StateDivision> list)
        {
            PagedResult<StateDivisionViewModel> vmList = new PagedResult<StateDivisionViewModel>();
            foreach (var data in list.data)
            {
                StateDivisionViewModel vm = new StateDivisionViewModel();
                vm.id = data.id;
                vm.name = data.name;
                if (data.CountryType != null)
                {
                    vm.country_type_name = data.CountryType.name;
                }
                if (data.Country != null)
                {
                    vm.country_name = data.Country.name;
                }
                vmList.data.Add(vm);
            }
            vmList.total = vmList.data.Count;
            return vmList;
        }

    }
}
