using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.DataAccess.Repositories.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Infrastruture.Utilities;
using DIS.Web.Controllers.Common;
using DIS.Web.Mappers.Setttings;
using DIS.Web.ViewModels;
using DMS.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NPOI.OpenXmlFormats.Dml.Diagram;

namespace DIS.Web.Controllers.Settings
{

    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : BaseController
    {
        ICountryRepository countryRepository;
        CountryMapper _mapper;
        Icountry_countrytypeRepository _cctrepo;
        public CountryController(ICountryRepository repository, Icountry_countrytypeRepository countrycountrytyperepo) : base(typeof(CountryController))
        {
            countryRepository = repository;
            _cctrepo = countrycountrytyperepo;
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
            PagedResult<CountryViewModel> vmList = _mapper.MapModelToListViewModel(list, _cctrepo);
            return vmList;

        }
        private CountryViewModel GetRequestParameter()
        {
            CountryViewModel vm = new CountryViewModel();

            vm.name = GetRequestParameter<string>("search[name]");
            // vm.CountryTypeListId = GetRequestParameter<int>("search[CountryTypeListId]");
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
                            List<country_countrytype> cctlist = _cctrepo.GetByCountryId(result.id);
                            foreach (var cct in cctlist)
                            {
                                _cctrepo.Remove(cct);
                            }

                            foreach (var typeId in vm.CountryTypeListId)
                            {
                                country_countrytype cctli = new country_countrytype();

                                cctli.country_id = result.id;
                                cctli.country_type_id = typeId;

                                _cctrepo.Save(cctli);
                            }
                        }
                        else
                        {
                            result.messages.Add(Constants.DuplicateMessage);
                        }
                    }
                }
                else
                {
                    Country? data = new Country();
                    if (!isDuplicate(data, vm))
                    {
                        data = _mapper.MapViewModelToModel(data, vm);
                        result = countryRepository.Save(data);

                        if (result.success)
                        {


                            foreach (var typeId in vm.CountryTypeListId)
                            {

                                var cct = new country_countrytype
                                {
                                    country_id = result.id,
                                    country_type_id = typeId
                                };

                                _cctrepo.Save(cct);
                            }

                        }

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
                if(data != null)
                {
                    var cctlist = _cctrepo.GetByCountryId(data.id);
                    vm = _mapper.MapModelToViewModel(data, vm, _cctrepo);
                }
       
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
        if (vm.name == data.name )
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

        if (dc != null && dc.name.Trim() == vm.name.Trim())
        {
            duplicate = true;
        }
    }
    return duplicate;


}

[HttpGet]
[Route("ExportExcel")]
public IActionResult ExportExcel()
{
    PagedResult<CountryViewModel> list = GetAllData();
    const string contentType = "application/octet-stream";
    HttpContext.Response.ContentType = contentType;
    HttpContext.Response.Headers.Add("attachment", "Content-Disposition");
    NPOISimpleExcelTable excel = new NPOISimpleExcelTable("Pyidaungsu", 13);
    excel.AddHeader("သဘာဝဘေးအန္တရာယ်စာရင်း");
    excel.AddColumn("စဉ်", typeof(string), NPOIExcelColumnWidth.S2);
    excel.AddColumn("နိုင်ငံအမျိုးအစား", typeof(string), NPOIExcelColumnWidth.M1);
    excel.AddColumn("နိုင်ငံ", typeof(string), NPOIExcelColumnWidth.M1);
    int count = 0;
    foreach (var item in list.data)
    {
        count++;
        excel.AddRow();
        excel.SetData(0, MyanmarEnglishConverter.ToMyanmarNumber(count.ToString()));
        excel.SetData(1, item.country_type_name);
        excel.SetData(2, item.name);



    }
    byte[] bytes = excel.Generate();
    var fileContentResult = new FileContentResult(bytes, contentType)
    {
        FileDownloadName = "Excel.xls"
    };
    return fileContentResult;
}

    }
}
