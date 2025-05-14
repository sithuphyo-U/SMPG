//using DIS.Infrastructure.Utilities;
//using DIS.Web.ViewModels;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DIS.DataAccess.Entity.Settings
//{
//    public class StateDivisionMapper
//    {
//        public QueryOptions<StateDivision> PrepareQueryOptionForRepository(QueryOptions<StateDivision> options, StateDivisionViewModel vm)
//        {
//            if (!string.IsNullOrEmpty(vm.name))
//            {
//                options.FilterBy = (x => x.name.Contains(vm.name));
//            }
//            if (options.SortColumnsName != null)
//            {
//                options.SortBy = new List<Func<StateDivision, object>>();
//                if (options.SortColumnName == "name")
//                {
//                    options.SortBy.Add((x => x.name));
//                }
//                else
//                {
//                    options.SortOrder = Infrastructure.Enumerations.SortOrder.DESC;
//                    options.SortBy.Add((x => x.id));
//                }
//            }
//            else
//            {
//                options.SortBy.Add((x => x.id));
//            }
//            return options;
//        }
//        public StateDivision? MapViewModelToModel(StateDivision? data, StateDivisionViewModel vm)
//        {
//            if (data != null)
//            {

//                data.name = vm.name;
//                if (vm.category_id > 0)
//                {
//                    data.disaster_category_id = vm.category_id;
//                }


//            }
//            return data;
//        }
//        public StateDivisionViewModel? MapModelToViewModel(StateDivision data, StateDivisionViewModel vm)
//        {
//            if (data != null)
//            {
//                vm.id = data.id;
//                vm.name = data.name;
//                if (data.disastercategory != null)
//                {
//                    vm.category_id = data.disaster_category_id;
//                    vm.category_name = data.disastercategory.name;
//                }
//            }
//            return vm;
//        }

//        public PagedResult<StateDivisionViewModel> MapModelToListViewModel(PagedResult<StateDivision> list)
//        {
//            PagedResult<StateDivisionViewModel> vmList = new PagedResult<StateDivisionViewModel>();
//            foreach (var data in list.data)
//            {
//                StateDivisionViewModel vm = new StateDivisionViewModel();
//                vm.id = data.id;
//                vm.name = data.name;
//                if (data.disastercategory != null)
//                {
//                    vm.category_name = data.disastercategory.name;
//                }
//                vmList.data.Add(vm);
//            }
//            vmList.total = vmList.data.Count;
//            return vmList;
//        }

//    }
//}
