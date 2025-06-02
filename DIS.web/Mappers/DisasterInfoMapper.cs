using DIS.DataAccess.Entity;
using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Infrastruture.Utilities;
using DIS.Web.ViewModels;
using System.Collections.Immutable;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DIS.Web.Mappers
{
    public class DisasterInfoMapper
    {

        public QueryOptions<DisasterInfo> PrepareQueryOptionForRepository(QueryOptions<DisasterInfo> options, DisasterInfoViewModel vm,IDisasterSubCategoryRepository repo)
        {
            if (vm.disaster_category_id > 0)
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => x.disasterCategory_id == vm.disaster_category_id);
            }
            if (vm.subcategory_id > 0)
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => x.subCategory_id == vm.subcategory_id);
            }
            if ((vm.disaster_category_id==0) && vm.subcategory_id > 0)
            {
                DisasterSubCategory Data = repo.Getbyid(vm.subcategory_id);

                List<DisasterSubCategory> sub = repo.GetByName(Data.name);
                foreach (var s in sub)
                {
                    if (options.FilterBy == null)
                    {
                        options.FilterBy = x => x.subCategory_id == s.id;
                    }
                    else
                    {
                        options.FilterBy = LinqExpressionHelper.AppendOr(options.FilterBy, x => x.subCategory_id == s.id);
                    }

                }

            }
                if (vm.country_type_id > 0)
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => x.country_type_id == vm.country_type_id);
            }
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
            if (vm.township_id > 0)
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => x.township_id == vm.township_id);
            }
            if (!string.IsNullOrEmpty(vm.title))
            {
                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => x.title.Contains(vm.title));
            }
            if (!string.IsNullOrEmpty(vm.from_date) && string.IsNullOrEmpty(vm.to_date))
            {
                DateTime fromdt = DateTime.ParseExact(vm.from_date.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => (x.date >= fromdt));
            }
            if (string.IsNullOrEmpty(vm.from_date) && !string.IsNullOrEmpty(vm.to_date))
            {
                DateTime todt = DateTime.ParseExact(vm.to_date.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy, x => (x.date <= todt));
            }
            if (!string.IsNullOrEmpty(vm.from_date) && !string.IsNullOrEmpty(vm.to_date))
            {

                DateTime fdate = DateTime.ParseExact(vm.from_date.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                DateTime tdate = DateTime.ParseExact(vm.to_date.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                options.FilterBy = LinqExpressionHelper.AppendAnd(options.FilterBy,
                    (a => a.date >= fdate && a.date <= tdate));

            }



            if (options.SortColumnsName != null)
            {
                options.SortBy = new List<Func<DisasterInfo, object>>();
                foreach (var SortColumnName in options.SortColumnsName)
                    if (SortColumnName == "title")
                {
                    options.SortBy.Add((x => x.title));
                }
                else if (SortColumnName == "disasterCategory_name")
                {
                    options.SortBy.Add((x => x.DisasterCategory.name));
                }
                else if (SortColumnName == "subCategory_name")
                {
                    options.SortBy.Add((x => x.DisasterCategory.name));
                }
                else if (SortColumnName == "country_type_name")
                {
                    options.SortBy.Add((x => x.CountryType.name));
                }
                else if (SortColumnName == "country_name")
                {
                    options.SortBy.Add((x => x.Country.name));
                }
                else if (SortColumnName == "state_division_name")
                {
                    options.SortBy.Add((x => x.StateDivision.name));
                }
                else if (SortColumnName == "district_name")
                {
                    options.SortBy.Add((x => x.District.name));
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
        public DisasterInfo? MapViewModelToModel(DisasterInfo? data, DisasterInfoViewModel vm)
        {
            if (data != null)
            {

                data.title = vm.title;
                data.time = vm.time;
                if (vm.disaster_category_id > 0)
                {
                    data.disasterCategory_id = vm.disaster_category_id;
                }
                if (vm.subcategory_id > 0)
                {
                    data.subCategory_id = vm.subcategory_id;
                }
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
                if ( vm.country_type_id > 0)
                {
                    data.country_type_id = vm.country_type_id;
                }
                if (vm.township_id > 0)
                {
                    data.township_id = vm.township_id;
                }
                if(vm.date != null)
                {
                    data.date = vm.date;
                }
                data.details = vm.totalCountofNews;
            }
            return data;
        }
        public DisasterInfoViewModel? MapModelToViewModel(DisasterInfo data, DisasterInfoViewModel vm,IFile_TBRepository repo)
        {
            if (data != null)
            {
                vm.id = data.id;
                vm.title = data.title;
                vm.date = data.date;
                vm.totalCountofNews = data.details;
                if (data.DisasterCategory != null)
                {
                    vm.disaster_category_id = data.disasterCategory_id;
                    vm.disasterCategory_name = data.DisasterCategory.name;
                }
                if (data.SubCategory != null)
                {
                    vm.subcategory_id = data.subCategory_id;
                    vm.subCategory_name = data.SubCategory.name;
                }

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
                if (data.District != null)
                {
                    vm.district_id = data.district_id;
                    vm.district_name = data.District.name;
                }
                if (data.Township != null)
                {
                    vm.township_id = data.township_id;
                    vm.township_name = data.Township.name;
                }
                if (data.id>0)
                {
                    List<File_TB> list = new List<File_TB>();
                    list = repo.GetFilebyDisasterInfoId(data.id);
                    foreach(var file in list)
                    {
                        FileViewModel fileViewModel = new FileViewModel();
                        fileViewModel.id = file.id;
                        fileViewModel.file_name = file.file_name;
                        fileViewModel.file_type = file.file_type;
                        fileViewModel.originalfile_name = file.originalfile_name;
                        fileViewModel.path = file.path;
                        vm.Files_List.Add(fileViewModel);
                    }
       
                    
                }


            }
            return vm;
        }
        public PagedResult<DisasterInfoViewModel> MapModelToListViewModel(PagedResult<DisasterInfo> list, IDisasterInfoFileRepository _disasterInfoFileRepo)
        {
            PagedResult<DisasterInfoViewModel> vmList = new PagedResult<DisasterInfoViewModel>();
            foreach (var data in list.data)
            {
                DisasterInfoViewModel vm = new DisasterInfoViewModel();
                vm.Files_List = new List<FileViewModel>();
                vm.id = data.id;
                vm.title = data.title;
                if(data.DisasterCategory != null)
                {
                    vm.disasterCategory_name = data.DisasterCategory.name;
                }
                if(data.SubCategory != null)
                {
                    vm.subCategory_name = data.SubCategory.name;
                }
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
                if (data.District != null)
                {
                    vm.district_name = data.District.name;
                }
                if (data.Township != null)
                {
                    vm.township_name = data.Township.name;
                }
                vm.date = data.date;
                vm.time = data.time;

                if (data.created_date != null)
                {
                    vm.created_date = data.created_date.Value.AddHours(24);
                }



                vm.totalCountofNews = data.details;
                List<File_TB> files = _disasterInfoFileRepo.GetDataById(vm.id);
                if (files.Count > 0)
                {

                    foreach (var file in files)
                    {
                        FileViewModel fileView = new FileViewModel
                        {
                            id = file.id,
                            originalfile_name = file.originalfile_name,
                            url = Constants.FilePath,
                            path = file.path,
                            file_name = file.file_name,
                            file_type = file.file_type

                        };
                        vm.Files_List.Add(fileView);
                    }
                }
                vmList.data.Add(vm);

            }
            //vmList.total = vmList.data.Count;
            vmList.total = list.total;
            vmList.success = list.success;
            vmList.messages = new List<string>(list.messages);
            return vmList;
        }

    }

}
