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
    public class CountryController : BaseController
    {
        ICountryRepository countryRepository;
        CountryMapper _mapper;
        public CountryController(ICountryRepository repository) : base(typeof(CountryController))
        {
            countryRepository = repository;
            _mapper = new CountryMapper();
        }
        [HttpGet]
        public JsonResult Get()
        {
            PagedResult<CountryViewModel> list = new PagedResult<CountryViewModel>();
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
        private PagedResult<CountryViewModel> GetAllData()
        {
            QueryOptions<Country> queryOptions = GetQueryOptions<Country>();
            CountryViewModel vm = GetRequestParameter();
            queryOptions = _mapper.PrepareQueryOptionForRepository(queryOptions, vm);
            PagedResult<Country> list = countryRepository.GetPagedResults(queryOptions);
            PagedResult<CountryViewModel> vmList = _mapper.MapModelToListViewModel(list);
            return vmList;

        }
        private CountryViewModel GetRequestParameter()
        {
            CountryViewModel vm = new CountryViewModel();
            vm.name = Request.Query["name"].ToString();
            return vm;
        }
        [HttpPost]
        [Route("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(CountryViewModel vm)
        {
            CommandResult<Country> result = new CommandResult<Country>();
            try
            {
                if (vm.id > 0)
                {
                    Country? data = countryRepository.Get(vm.id);
                    if (!isDuplicate(data, vm))
                    {
                        data = _mapper.MapViewModelToModel(data, vm);
                        result = countryRepository.Save(data);
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
                    Country? data = new Country();
                    data = _mapper.MapViewModelToModel(data, vm);
                    result = countryRepository.Save(data);

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
            CommandResult<Country> result = new CommandResult<Country>();
            try
            {
                Country? data = countryRepository.Get(id);
                if (data != null)
                {
                    result = countryRepository.Remove(data);
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
            CountryViewModel vm = new CountryViewModel();
            try
            {
                Country? data = countryRepository.Get(id);
                vm = _mapper.MapModelToViewModel(data, vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(vm);
        }

        protected bool isDuplicate(Country data, CountryViewModel vm)
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
                    Country? dc = countryRepository.FindByName(vm.name);
                    if (dc != null)
                    {
                        duplicate = true;
                    }
                }
            }
            else
            {
                Country? dc = countryRepository.FindByName(vm.name);
                if (dc != null)
                {
                    duplicate = true;
                }
            }
            return duplicate;


        }

    }
}
