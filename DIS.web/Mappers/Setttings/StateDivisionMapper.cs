using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Enumerations;
using DIS.Infrastructure.Utilities;
using DIS.Infrastruture.Utilities;
using DIS.Web.ViewModels;
using System.Linq.Expressions;

namespace DIS.Web.Mappers.Setttings
{
    public class StateDivisionMapper
    {
        public QueryOptions<StateDivision> PrepareQueryOptionForRepository(QueryOptions<StateDivision> options, StateDivisionViewModel vm)
        {
            if (vm.cc_type != null && vm.cc_type.Count > 0)
            {
                List<int> countryIds = vm.cc_type.Select(c => c.country_id).ToList();

                Expression<Func<StateDivision, bool>> combinedFilter = null;

                foreach (var cid in countryIds)
                {
                    Expression<Func<StateDivision, bool>> singleFilter = x => x.country_id == cid;

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
            if (!string.IsNullOrEmpty(vm.name))
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => x.name.Contains(vm.name));
            }


            if (options.SortColumnName != null)
            {
                options.SortBy = new List<Func<StateDivision, object>>();
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


        public StateDivision? MapViewModelToModel(StateDivision? data, StateDivisionViewModel vm)
        {
            if (data != null)
            {

                data.name = vm.name;
                
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
            }
            return vm;
        }

        public PagedResult<StateDivisionViewModel> MapModelToListViewModel(PagedResult<StateDivision> list, Icountry_countrytypeRepository _cctrepo,IStateDivisionRepository _staterepo)
        {
            PagedResult<StateDivisionViewModel> vmList = new PagedResult<StateDivisionViewModel>();
            foreach (var data in list.data)
            {
                StateDivisionViewModel vm = new StateDivisionViewModel();
                vm.id = data.id;
                vm.name = data.name;
                if (data.Country != null)
                {
                    vm.country_name = data.Country.name;
                }
                StateDivision? statedivision = _staterepo.GetCountryByStateDivision(data.id);



                List<country_countrytype> cctlist = _cctrepo.GetByCountryId(statedivision.country_id);
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
