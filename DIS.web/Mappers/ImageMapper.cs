using DIS.DataAccess.Entity;
using DIS.Infrastructure.Utilities;
using DIS.Web.ViewModels;

namespace DIS.Web.Mappers
{
    public class ImageMapper
    {
        public QueryOptions<Image> PrepareQueryOptionForRepository(QueryOptions<Image> options, ImageViewModel vm)
        {


            return options;
        }
        public Image? MapViewModelToModel(Image? data, ImageViewModel vm)
        {
            if (data != null)
            {
                Guid guId = Guid.NewGuid();
                data.image_name = vm.image_name;
                data.image_type = vm.image_type;
                data.original_image_name = vm.original_image_name;
                


            }
            return data;
        }
        public List<ImageViewModel> MapModelToListViewModel(List<Image> list)
        {
            List<ImageViewModel> vmList = new List<ImageViewModel>();
            foreach (var data in list)
            {
                ImageViewModel vm = new ImageViewModel();
                vm.imageId = data.id;
                vm.image_name = data.image_name;
                vm.image_type = data.image_type;
                vm.original_image_name = data.original_image_name;
                vm.path = data.path;


                vmList.Add(vm);
            }
            //vmList.total = vmList.data.Count;

            return vmList;
        }

    }
}
