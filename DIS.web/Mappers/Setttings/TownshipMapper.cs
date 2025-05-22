using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Enumerations;
using DIS.Infrastructure.Utilities;
using DIS.Infrastruture.Utilities;
using DIS.Web.ViewModels;

namespace DIS.Web.Mappers.Setttings
{
    public class TownshipMapper
    {
        public QueryOptions<Township> PrepareQueryOptionForRepository(QueryOptions<Township> options, TownshipViewModel vm)
        {
            //if (vm.country_type_id > 0)
            //{
            //    options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => x.country_type_id == vm.country_type_id);
            //}
            if (vm.country_id > 0)
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => x.country_id == vm.country_id);
            }
            if (vm.state_division_id > 0)
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => x.state_division_id == vm.state_division_id);
            }
            if (vm.district_id > 0)
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => x.district_id == vm.district_id);
            }
            if (!string.IsNullOrEmpty(vm.name))
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => x.name.Contains(vm.name));
            }



            if (options.SortColumnName != null)
            {
                options.SortBy = new List<Func<Township, object>>();
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
                else if (options.SortColumnName == "district_name")
                {
                    options.SortBy.Add((x => x.District.name));
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


        public Township? MapViewModelToModel(Township? data, TownshipViewModel vm)
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
                if (vm.district_id > 0)
                {
                    data.district_id = vm.district_id;
                }

            }
            return data;
        }
        public TownshipViewModel? MapModelToViewModel(Township data, TownshipViewModel vm)
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
                if (data.District != null)
                {
                    vm.district_id = data.district_id;
                    vm.district_name = data.District.name;
                }

            }
            return vm;
        }
        public PagedResult<TownshipViewModel> MapModelToListViewModel(PagedResult<Township> list, Icountry_countrytypeRepository _cctrepo,ITownshipRepository _townshiprepo)
        {
            PagedResult<TownshipViewModel> vmList = new PagedResult<TownshipViewModel>();
            foreach (var data in list.data)
            {
                TownshipViewModel vm = new TownshipViewModel();
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
                if (data.District != null)
                {
                    vm.district_name = data.District.name;
                }


                Township? township = _townshiprepo.GetCountryByTownship(data.id);

                List<country_countrytype> cctlist = _cctrepo.GetByCountryId(township.country_id);
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
