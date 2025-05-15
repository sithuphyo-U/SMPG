using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.DataAccess.Repositories.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Web.Controllers.Common;
using DIS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DIS.Web.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : BaseController
    {
        IDistrictRepository districtRepository;
        DistrictMapper _mapper;
        public DistrictController(IDistrictRepository type) : base(typeof(DistrictController))
        {
            districtRepository = type;
            _mapper = new DistrictMapper();
        }

        [HttpGet]
        public JsonResult Get()
        {
            PagedResult<DistrictViewModel> list = new PagedResult<DistrictViewModel>();
            try
            {
                list = GetAllData();
            }
            catch (Exception ex)
            {
                list.success = false;
                list.messages.Add(ex.Message);
                logger.LogError(ex.Message);

            }
            return Json(list);
        }
        private PagedResult<DistrictViewModel> GetAllData()
        {
            QueryOptions<District> queryOptions = GetQueryOptions<District>();
            DistrictViewModel vm = GetRequestParameter();
            queryOptions = _mapper.PrepareQueryOptionForRepository(queryOptions, vm);
            PagedResult<District> list = districtRepository.GetPagedResults(queryOptions);
            PagedResult<DistrictViewModel> vmList = _mapper.MapModelToListViewModel(list);
            return vmList;

        }
        private DistrictViewModel GetRequestParameter()
        {
            DistrictViewModel vm = new DistrictViewModel();
            vm.name = GetRequestParameter<string>("search[name]");
            vm.country_type_id = GetRequestParameter<int>("search[country_type_id]");
            vm.country_id = GetRequestParameter<int>("search[country_id]");
            vm.state_division_id = GetRequestParameter<int>("search[state_division_id]");


            return vm;
        }

        [HttpPost]
        [Route("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(DistrictViewModel vm)
        {
            CommandResult<District> result = new CommandResult<District>();
            try
            {
                if (vm.id > 0)
                {
                    District? data = districtRepository.Get(vm.id);
                    if (!isDuplicate(data, vm))
                    {
                        data = _mapper.MapViewModelToModel(data, vm);
                        result = districtRepository.Save(data);
                        if (result.success)
                        {

                        }
                    }
                    else
                    {
                        result.messages.Add(Constants.DuplicateMessage);
                    }

                }
                else
                {
                    District? data = new District();
                    data = _mapper.MapViewModelToModel(data, vm);
                    result = districtRepository.Save(data);

                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.messages.Add(ex.Message);
                logger.LogError(ex.Message);

            }
            return Json(result);
        }
        [HttpDelete]
        [Route("delete/")]
        public JsonResult Delete(int id)
        {
            CommandResult<District> result = new CommandResult<District>();
            try
            {
                District? data = districtRepository.Get(id);
                if (data != null)
                {
                    result = districtRepository.Remove(data);
                    if (result.success)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(result);
        }
        [HttpGet]
        [Route("getbyid/")]
        public JsonResult GetById(int id)
        {
            DistrictViewModel vm = new DistrictViewModel();
            try
            {
                District? data = districtRepository.Get(id);
                vm = _mapper.MapModelToViewModel(data, vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(vm);
        }

        protected bool isDuplicate(District data, DistrictViewModel vm)
        {
            bool duplicate = false;
            if (data.id > 0)
            {
                if (vm.name == data.name)
                {
                    duplicate = false;

                }
                else
                {
                    District? dc = districtRepository.FindByName(vm.name);
                    if (dc != null)
                    {
                        duplicate = true;
                    }
                }
            }
            else
            {
                District? dc = districtRepository.FindByName(vm.name);
                if (dc != null)
                {
                    duplicate = true;
                }
            }
            return duplicate;


        }

    }
}
