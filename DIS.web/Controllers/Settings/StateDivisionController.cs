using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.DataAccess.Repositories.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Web.Controllers.Common;
using DIS.Web.Mappers.Setttings;
using DIS.Web.ViewModels;

using Microsoft.AspNetCore.Mvc;


namespace DIS.Web.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]

    public class StateDivisionController : BaseController
    {
        IStateDivisionRepository _repository;
        StateDivisionMapper _mapper;
        public StateDivisionController(IStateDivisionRepository _repository) : base(typeof(StateDivisionController))
        {
            _repository = _repository;
            _mapper = new StateDivisionMapper();
        }
        [HttpGet]
        public JsonResult Get()
        {
            PagedResult<StateDivisionViewModel> list = new PagedResult<StateDivisionViewModel>();
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
        private PagedResult<StateDivisionViewModel> GetAllData()
        {
            QueryOptions<StateDivision> queryOptions = GetQueryOptions<StateDivision>();
            StateDivisionViewModel vm = GetRequestParameter();
            queryOptions = _mapper.PrepareQueryOptionForRepository(queryOptions, vm);
            PagedResult<StateDivision> list = _repository.GetPagedResults(queryOptions);
            PagedResult<StateDivisionViewModel> vmList = _mapper.MapModelToListViewModel(list);
            return vmList;

        }
        private StateDivisionViewModel GetRequestParameter()
        {
            StateDivisionViewModel vm = new StateDivisionViewModel();
            vm.name = Request.Query["name"].ToString();
            return vm;
        }
        [HttpPost]
        [Route("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(StateDivisionViewModel vm)
        {
            CommandResult<StateDivision> result = new CommandResult<StateDivision>();
            try
            {
                if (vm.id > 0)
                {
                    StateDivision? data = _repository.Get(vm.id);
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
                    StateDivision? data = new StateDivision();
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
            CommandResult<StateDivision> result = new CommandResult<StateDivision>();
            try
            {
                StateDivision? data = _repository.Get(id);
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
            StateDivisionViewModel vm = new StateDivisionViewModel();
            try
            {
                StateDivision? data = _repository.Get(id);
                vm = _mapper.MapModelToViewModel(data, vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(vm);
        }

        protected bool isDuplicate(StateDivision data, StateDivisionViewModel vm)
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
                    StateDivision? dc = _repository.FindByName(vm.name);
                    if (dc != null)
                    {
                        duplicate = true;
                    }
                }
            }
            else
            {
                StateDivision? dc = _repository.FindByName(vm.name);
                if (dc != null)
                {
                    duplicate = true;
                }
            }
            return duplicate;


        }

    }
}
