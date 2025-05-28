using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Web.Controllers.Common;
using DIS.Web.Mappers.Setttings;
using DIS.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System.Linq.Expressions;

namespace DIS.Web.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class DisasterCategoryController : BaseController
    {
        IDisasterCategoryRepository _repository;
        DisasterCategoryMapper _mapper;
        public DisasterCategoryController(IDisasterCategoryRepository repository)
           : base(typeof(DisasterCategoryController))
        {
            _repository = repository;
            _mapper = new DisasterCategoryMapper();
        }

        [HttpGet]
        public JsonResult Get()
        {
            PagedResult<DisasterCategoryViewModel> list = new PagedResult<DisasterCategoryViewModel>();
            try
            {
                list = GetAllData();
            }   
            catch(Exception ex)
            {
                list.success = false;
                list.messages.Add(ex.Message);
                logger.LogError(ex.Message);

            }
            return Json(list);
        }
        private PagedResult<DisasterCategoryViewModel> GetAllData()
        {
            QueryOptions<DisasterCategory> queryOptions = GetQueryOptions<DisasterCategory>();
            DisasterCategoryViewModel vm = GetRequestParameter();
            queryOptions = _mapper.PrepareQueryOptionForRepository(queryOptions, vm);
            PagedResult<DisasterCategory> list = _repository.GetPagedResults(queryOptions);
            PagedResult<DisasterCategoryViewModel> vmList = _mapper.MapModelToListViewModel(list);
            return vmList;

        }
        private DisasterCategoryViewModel GetRequestParameter()
        {
            DisasterCategoryViewModel vm = new DisasterCategoryViewModel();
            vm.name = Request.Query["search[name]"].ToString();

            return vm;
        }


        [HttpPost]
        [Route("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(DisasterCategoryViewModel vm)
        {
            CommandResult<DisasterCategory> result = new CommandResult<DisasterCategory>();
            try
            {
                if (vm.id > 0)
                {
                    DisasterCategory? data = _repository.Get(vm.id);
                    if (!isDuplicate(data, vm))
                    {
                        data = _mapper.MapViewModelToModel(data, vm);
                        result = _repository.Save(data);
                        if (result.success)
                        {
                           // AuditLog(nameof(DisasterCategory), nameof(Position), Constants.UpdateAction);
                        }
                    }
                    else
                    {
                        result.messages.Add(Constants.DuplicateMessage);
                    }

                }
                else
                {
                    DisasterCategory? data = new DisasterCategory();
                    if (!isDuplicate(data, vm))
                    {
                        data = _mapper.MapViewModelToModel(data, vm);
                        result = _repository.Save(data);
                    }
                    else
                    {
                        result.messages.Add(Constants.DuplicateMessage);
                    }

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
        [HttpGet]
        [Route("getbyid/")]
        public JsonResult GetById(int id)
        {
            DisasterCategoryViewModel vm = new DisasterCategoryViewModel();
            try
            {
                DisasterCategory? data = _repository.Get(id);
                vm = _mapper.MapModelToViewModel(data, vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(vm);
        }
        [HttpDelete]
        [Route("delete/")]
        public JsonResult Delete(int id)
        {
            CommandResult<DisasterCategory> result = new CommandResult<DisasterCategory>();
            try
            {
                DisasterCategory? data = _repository.Get(id);
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
        protected bool isDuplicate(DisasterCategory data, DisasterCategoryViewModel vm)
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
                    DisasterCategory? dc = _repository.FindByName(vm.name);
                    if (dc != null)
                    {
                        duplicate = true;
                    }
                }
            }
            else
            {
                DisasterCategory? dc = _repository.FindByName(vm.name);
                if (dc != null)
                {
                    duplicate = true;
                }
            }
            return duplicate;


        }
    }
}
