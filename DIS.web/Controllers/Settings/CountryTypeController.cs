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

    public class CountryTypeController : BaseController
    {
        ICountryTypeRepository _countryTypeRepository;
        CountryTypeMapper _mapper;
        public CountryTypeController(ICountryTypeRepository countrytype) : base(typeof(CountryTypeController))
        {
            _countryTypeRepository = countrytype;
            _mapper = new CountryTypeMapper();
        }
        [HttpGet]
        public JsonResult Get()
        {
            PagedResult<CountryTypeViewModel> list = new PagedResult<CountryTypeViewModel>();
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
        private PagedResult<CountryTypeViewModel> GetAllData()
        {
            QueryOptions<CountryType> queryOptions = GetQueryOptions<CountryType>();
            CountryTypeViewModel vm = GetRequestParameter();
            queryOptions = _mapper.PrepareQueryOptionForRepository(queryOptions, vm);
            PagedResult<CountryType> list = _countryTypeRepository.GetPagedResults(queryOptions);
            PagedResult<CountryTypeViewModel> vmList = _mapper.MapModelToListViewModel(list);
            return vmList;

        }
        private CountryTypeViewModel GetRequestParameter()
        {
            CountryTypeViewModel vm = new CountryTypeViewModel();
            vm.name = Request.Query["search[name]"].ToString();

            return vm;
        }


        [HttpPost]
        [Route("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(CountryTypeViewModel vm)
        {
            CommandResult<CountryType> result = new CommandResult<CountryType>();
            try
            {
                if (vm.id > 0)
                {
                    CountryType? data = _countryTypeRepository.Get(vm.id);
                    if (!isDuplicate(data, vm))
                    {
                        data = _mapper.MapViewModelToModel(data, vm);
                        result = _countryTypeRepository.Save(data);
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
                    CountryType? data = new CountryType();
                    data = _mapper.MapViewModelToModel(data, vm);
                    result = _countryTypeRepository.Save(data);

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
            CommandResult<CountryType> result = new CommandResult<CountryType>();
            try
            {
                CountryType? data = _countryTypeRepository.Get(id);
                if (data != null)
                {
                    result = _countryTypeRepository.Remove(data);
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
            CountryTypeViewModel vm = new CountryTypeViewModel();
            try
            {
                CountryType? data = _countryTypeRepository.Get(id);
                vm = _mapper.MapModelToViewModel(data, vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(vm);
        }

        protected bool isDuplicate(CountryType data, CountryTypeViewModel vm)
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
                    CountryType? dc = _countryTypeRepository.FindByName(vm.name);
                    if (dc != null)
                    {
                        duplicate = true;
                    }
                }
            }
            else
            {
                CountryType? dc = _countryTypeRepository.FindByName(vm.name);
                if (dc != null)
                {
                    duplicate = true;
                }
            }
            return duplicate;


        }
    }
}
