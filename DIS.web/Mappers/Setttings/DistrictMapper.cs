using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Enumerations;
using DIS.Infrastructure.Utilities;
using DIS.Infrastruture.Utilities;
using System.Linq.Expressions;

namespace DIS.Web.ViewModels
{
    public class DistrictMapper
    {
        public QueryOptions<District> PrepareQueryOptionForRepository(QueryOptions<District> options, DistrictViewModel vm)
        {
            if (vm.cc_type != null && vm.cc_type.Count > 0)
            {
                List<int> countryIds = vm.cc_type.Select(c => c.country_id).ToList();

                Expression<Func<District, bool>> combinedFilter = null;

                foreach (var cid in countryIds)
                {
                    Expression<Func<District, bool>> singleFilter = x => x.id == cid;

                    if (combinedFilter == null)
                    {
                        combinedFilter = singleFilter;
                    }
                    else
                    {
                        combinedFilter = LinqExpressionHelper.AppendOr(combinedFilter, singleFilter);
                    }
                }

                if (combinedFilter != null)
                {
                    options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, combinedFilter);
                }
            }

            if (vm.country_id > 0)
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => x.country_id == vm.country_id);
            }
            if (vm.state_division_id > 0)
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => x.state_division_id == vm.state_division_id);
            }
            if (!string.IsNullOrEmpty(vm.name))
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => x.name.Contains(vm.name));
            }



            if (options.SortColumnName != null)
            {
                options.SortBy = new List<Func<District, object>>();
                if (options.SortColumnName == "name")
                {
                    options.SortBy.Add((x => x.name));
                }
                //else if (options.SortColumnName == "country_type_name")
                //{
                //    options.SortBy.Add((x => x.CountryType.name));
                //}
                else if (options.SortColumnName == "country_name")
                {
                    options.SortBy.Add((x => x.Country.name));
                }
                else if (options.SortColumnName == "state_division_name")
                {
                    options.SortBy.Add((x => x.StateDivision.name));
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


        public District? MapViewModelToModel(District? data, DistrictViewModel vm)
        {
            if (data != null)
            {

                data.name = vm.name;

                //if (vm.country_type_id > 0)
                //{
                //    data.country_type_id = vm.country_type_id;
                //}
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
                //if (data.CountryType != null)
                //{
                //    vm.country_type_id = data.country_type_id;
                //    vm.country_type_name = data.CountryType.name;
                //}
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

        public PagedResult<DistrictViewModel> MapModelToListViewModel(PagedResult<District> list, Icountry_countrytypeRepository _cctrepo, IDistrictRepository _districtrepo)
        {
            PagedResult<DistrictViewModel> vmList = new PagedResult<DistrictViewModel>();
            foreach (var data in list.data)
            {
                DistrictViewModel vm = new DistrictViewModel();
                vm.id = data.id;
                vm.name = data.name;
              
                if (data.Country != null)
                {
                    vm.country_name = data.Country.name;
                }
                if (data.StateDivision != null)
                {
                    vm.state_division_name = data.StateDivision.name;
                }

                District? district = _districtrepo.GetCountryByDistrict(data.id);


                List<country_countrytype> cctlist = _cctrepo.GetByCountryId(district.country_id);
                string cctlists = string.Empty;
                int count = 0;
                foreach (var cct in cctlist)
                {
                    count++;
                    if (count == 1)
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
