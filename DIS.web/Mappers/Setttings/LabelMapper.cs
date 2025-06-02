using DIS.DataAccess.Entity;
using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Web.ViewModels;
using Newtonsoft.Json;

namespace DIS.Web.Mappers.Setttings
{
    public class LabelMapper
    {
        public QueryOptions<Label> PrepareQueryOptionForRepository(QueryOptions<Label> options, LabelViewModel vm)
        {

            
            return options;
        }

        public Label? MapViewModelToModel(Label? data, LabelViewModel vm)
        {
            if (data != null)
            {

                data.category_name = vm.category_name;
                data.sub_category = vm.sub_category;
                data.country_type = vm.country_type;
                data.country = vm.country;
                data.statedivison = vm.statedivison;
                data.district = vm.district;
                data.township = vm.township;
                data.casedate = vm.casedate;
                data.File_Name = vm.file;
               
                data.headline = vm.headline;
                data.totalNews = vm.totalNews;



            }
            return data;
        }
        public List<LabelViewModel> MapModelToListViewModel(List<Label> list)
        {
            List<LabelViewModel> vmList = new List<LabelViewModel>();
            foreach (var data in list)
            {
                LabelViewModel vm = new LabelViewModel();
                vm.id = data.id;
                vm.category_name = data.category_name;
                vm.sub_category = data.sub_category;              
                vm.country_type = data.country_type;
                vm.country = data.country;
                vm.statedivison = data.statedivison;
                vm.district = data.district;
                vm.township = data.township;
                vm.totalNews = data.totalNews;
                vm.headline = data.headline;
                vm.file = data.File_Name;
                vm.casedate = data.casedate;
                vm.totalNews = data.totalNews;

                vmList.Add(vm);
            }
            //vmList.total = vmList.data.Count;
           
            return vmList;
        }
       
    }
}
