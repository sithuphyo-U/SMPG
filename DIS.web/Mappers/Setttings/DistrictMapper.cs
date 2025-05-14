using DIS.DataAccess.Entity.Settings;
using DIS.Infrastructure.Utilities;

namespace DIS.Web.ViewModels
{
    public class DistrictMapper
    {
        public QueryOptions<District> PrepareQueryOptionForRepository(QueryOptions<District> options, DistrictViewModel vm)
        {
            if (!string.IsNullOrEmpty(vm.name))
            {
                options.FilterBy = (x => x.name.Contains(vm.name));
            }
            if (options.SortColumnsName != null)
            {
                options.SortBy = new List<Func<District, object>>();
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
        public District? MapViewModelToModel(District? data, DistrictViewModel vm)
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
                if (vm.state_division_id > 0)
                {
                    data.state_division_id = vm.state_division_id;
                }


            }
            return data;
        }
        public DistrictViewModel? MapModelToViewModel(District data, DistrictViewModel vm)
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
                if (data.StateDivision != null)
                {
                    vm.state_division_id = data.state_division_id;
                    vm.state_division_name = data.StateDivision.name;
                }

            }
            return vm;
        }

        public PagedResult<DistrictViewModel> MapModelToListViewModel(PagedResult<District> list)
        {
            PagedResult<DistrictViewModel> vmList = new PagedResult<DistrictViewModel>();
            foreach (var data in list.data)
            {
                DistrictViewModel vm = new DistrictViewModel();
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
                if (data.StateDivision != null)
                {
                    vm.state_division_name = data.StateDivision.name;
                }
                vmList.data.Add(vm);
            }
            vmList.total = vmList.data.Count;
            return vmList;
        }

    }
}
