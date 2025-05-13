
using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Web.Controllers.Common;
using DIS.Web.Mappers.Setttings;
using DIS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace DIS.Web.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisasterSubCategoryController : BaseController
    {
        IDisasterSubCategoryRepository _repository;
        DisasterSubCategoryMapper _mapper;

        public DisasterSubCategoryController(IDisasterSubCategoryRepository repository) : base(typeof(DisasterSubCategoryController))
        {
            _repository = repository;
            _mapper = new DisasterSubCategoryMapper();


        }
        [HttpGet]
        public JsonResult Get()
        {
            PagedResult<DisasterSubCategoryViewModel> list = new PagedResult<DisasterSubCategoryViewModel>();
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
        private PagedResult<DisasterSubCategoryViewModel> GetAllData()
        {
            QueryOptions<DisasterSubCategory> queryOptions = GetQueryOptions<DisasterSubCategory>();
            DisasterSubCategoryViewModel vm = GetRequestParameter();
            queryOptions = _mapper.PrepareQueryOptionForRepository(queryOptions, vm);
            PagedResult<DisasterSubCategory> list = _repository.GetPagedResults(queryOptions);
            PagedResult<DisasterSubCategoryViewModel> vmList = _mapper.MapModelToListViewModel(list);
            return vmList;

        }
        private DisasterSubCategoryViewModel GetRequestParameter()
        {
            DisasterSubCategoryViewModel vm = new DisasterSubCategoryViewModel();
            vm.name = Request.Query["name"].ToString();
            return vm;
        }
        [HttpPost]
        [Route("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(DisasterSubCategoryViewModel vm)
        {
            CommandResult<DisasterSubCategory> result = new CommandResult<DisasterSubCategory>();
            try
            {
                if (vm.id > 0)
                {
                    DisasterSubCategory? data = _repository.Get(vm.id);
                    if (!isDuplicate(data, vm))
                    {
                        data = _mapper.MapViewModelToModel(data, vm);
                        result = _repository.Save(data);
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
                    DisasterSubCategory? data = new DisasterSubCategory();
                    data = _mapper.MapViewModelToModel(data, vm);
                    result = _repository.Save(data);

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
            CommandResult<DisasterSubCategory> result = new CommandResult<DisasterSubCategory>();
            try
            {
                DisasterSubCategory? data = _repository.Get(id);
                if (data != null)
                {
                    result = _repository.Remove(data);
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
            DisasterSubCategoryViewModel vm = new DisasterSubCategoryViewModel();
            try
            {
                DisasterSubCategory? data = _repository.Get(id);
                vm = _mapper.MapModelToViewModel(data, vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(vm);
        }

        protected bool isDuplicate(DisasterSubCategory data, DisasterSubCategoryViewModel vm)
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
                    DisasterSubCategory? dc = _repository.FindByName(vm.name);
                    if (dc != null)
                    {
                        duplicate = true;
                    }
                }
            }
            else
            {
                DisasterSubCategory? dc = _repository.FindByName(vm.name);
                if (dc != null)
                {
                    duplicate = true;
                }
            }
            return duplicate;


        }
    }
}
