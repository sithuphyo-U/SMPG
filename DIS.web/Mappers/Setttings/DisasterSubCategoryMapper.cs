using DIS.DataAccess.Entity.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Infrastruture.Utilities;
using DIS.Web.ViewModels;
using System;
using System.Collections.Generic;

namespace DIS.Web.Mappers.Setttings
{
    public class DisasterSubCategoryMapper
    {
        public QueryOptions<DisasterSubCategory> PrepareQueryOptionForRepository(QueryOptions<DisasterSubCategory> options, DisasterSubCategoryViewModel vm)
        {
            if (vm.category_id > 0)
            {
                options.FilterBy = ( x => x.disaster_category_id == vm.category_id );
            }
            if (!string.IsNullOrEmpty(vm.name))
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, (x => x.name.Contains(vm.name)));
            }
            if (options.SortColumnsName != null)
            {
                options.SortBy = new List<Func<DisasterSubCategory, object>>();
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
        public DisasterSubCategory? MapViewModelToModel(DisasterSubCategory? data, DisasterSubCategoryViewModel vm)
        {
            if (data != null)
            {   

                data.name = vm.name;
                if(vm.category_id>0)
                {
                    data.disaster_category_id = vm.category_id;
                }
                

            }
            return data;
        }
        public DisasterSubCategoryViewModel? MapModelToViewModel(DisasterSubCategory data, DisasterSubCategoryViewModel vm)
        {
            if (data != null)
            {
                vm.id = data.id;
                vm.name = data.name;
                if(data.disastercategory !=null)
                {
                    vm.category_id = data.disaster_category_id;
                    vm.category_name = data.disastercategory.name;
                }
            }
            return vm;
        }

        public PagedResult<DisasterSubCategoryViewModel> MapModelToListViewModel(PagedResult<DisasterSubCategory> list)
        {
            PagedResult<DisasterSubCategoryViewModel> vmList = new PagedResult<DisasterSubCategoryViewModel>();
            foreach (var data in list.data)
            {
                DisasterSubCategoryViewModel vm = new DisasterSubCategoryViewModel();
                vm.id = data.id;
                vm.name = data.name;
                if (data.disastercategory != null)
                {
                    vm.category_name= data.disastercategory.name;
                }
                vmList.data.Add(vm);
            }
            vmList.total = vmList.data.Count;
            return vmList;
        }

    }
}
