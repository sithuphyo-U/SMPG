using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Enumerations;
using DIS.Infrastructure.Utilities;
using DIS.Infrastruture.Utilities;
using DIS.Web.ViewModels;
using NPOI.Util;

namespace DIS.Web.Mappers.Setttings
{
    public class CountryMapper
    {

        public QueryOptions<Country> PrepareQueryOptionForRepository(QueryOptions<Country> options, CountryViewModel vm)
        {
            if (vm.id > 0)
            {
                options.FilterBy = (x => x.id == vm.id);
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
                //else if (options.SortColumnName == "country_type_name")
                //{
                //    options.SortBy.Add((x => x.CountryType.name));
                //}
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

            }

            return data;
        }

        public CountryViewModel? MapModelToViewModel(Country data, CountryViewModel vm, Icountry_countrytypeRepository _cctrepo)
        {
            if (data != null)
            {
                vm.id = data.id;
                vm.name = data.name;
                vm.CountryTypeListId = _cctrepo.GetByCountryId(data.id).Select(m => m.country_type_id).ToList();
                vm.country_type_name = _cctrepo.GetByCountryId(data.id).Select(m => m.CountryType.name).ToList();



            }
            return vm;
        }



        public PagedResult<CountryViewModel> MapModelToListViewModel(PagedResult<Country> list, Icountry_countrytypeRepository _cctrepo)
        {
            PagedResult<CountryViewModel> vmList = new PagedResult<CountryViewModel>();
            foreach (var data in list.data)
            {
                CountryViewModel vm = new CountryViewModel();
                vm.id = data.id;
                vm.name = data.name;
               
                List<country_countrytype> cctlist = _cctrepo.GetByCountryId(data.id);
                string cctlists = string.Empty;
                int count = 0;
                foreach(var cct in cctlist)
                {
                    count++;
                    if(count == 1)
                    {
                        cctlists = cct.CountryType.name;
                    }
                    else
                    {
                        cctlists = cctlists + " , " + cct.CountryType.name;
                    }
                }
                vm.countryType_name = cctlists;
                
                vmList.data.Add(vm);
            }
            vmList.total = vmList.data.Count;
            return vmList;
        }

    }
}
