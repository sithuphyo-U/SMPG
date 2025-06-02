using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.DataAccess.Repositories.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Infrastruture.Enumerations;
using DIS.Infrastruture.Utilities;
using DIS.Web.Controllers.Common;
using DIS.Web.Mappers.Setttings;
using DIS.Web.ViewModels;
using DMS.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DIS.Web.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
   [Authorize]
    public class TownshipController : BaseController
    {
        ITownshipRepository _repository;
        Icountry_countrytypeRepository _cctrepo;
        ITownshipRepository _townshiprepo;
        TownshipMapper _mapper;
        public TownshipController(ITownshipRepository townshipRepository,Icountry_countrytypeRepository icountry_CountrytypeRepository, ITownshipRepository townshiprepository) : base(typeof(TownshipController))
        {
            _repository = townshipRepository;
            _cctrepo = icountry_CountrytypeRepository;
            _townshiprepo = townshipRepository;
            _mapper = new TownshipMapper();
        }
        [HttpGet]
        public JsonResult Get()
        {
            PagedResult<TownshipViewModel> list = new PagedResult<TownshipViewModel>();
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
        private PagedResult<TownshipViewModel> GetAllData()
        {
            QueryOptions<Township> queryOptions = GetQueryOptions<Township>();
            TownshipViewModel vm = GetRequestParameter();
            queryOptions = _mapper.PrepareQueryOptionForRepository(queryOptions, vm);
            PagedResult<Township> list = _repository.GetPagedResults(queryOptions);
            PagedResult<TownshipViewModel> vmList = _mapper.MapModelToListViewModel(list,_cctrepo, _townshiprepo);
            return vmList;

        }
        private TownshipViewModel GetRequestParameter()
              {
            TownshipViewModel vm = new TownshipViewModel();
            vm.name = GetRequestParameter<string>("search[name]");
            vm.country_type_id = GetRequestParameter<int>("search[country_type_id]");
            if (vm.country_type_id > 0)
            {
                List<country_countrytype> cc = new List<country_countrytype>();
                cc = _cctrepo.GetByCountryTypeByCountryId(vm.country_type_id);
                vm.cc_type = cc;

            }
            vm.country_id = GetRequestParameter<int>("search[country_id]");
            vm.state_division_id = GetRequestParameter<int>("search[state_division_id]");
            vm.district_id = GetRequestParameter<int>("search[district_id]");
            return vm;
        }

        [HttpPost]
        [Route("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(TownshipViewModel vm)
        {
            CommandResult<Township> result = new CommandResult<Township>();
            try
            {
                if (vm.id > 0)
                {
                    Township? data = _repository.Get(vm.id);
                    if (!isDuplicate(data, vm))
                    {
                        data = _mapper.MapViewModelToModel(data, vm);
                        result = _repository.Save(data);
                        if (result.success)
                        {
                            AuditLog(nameof(TownshipController), nameof(Township), AuditAction.UPDATE.ToString());
                        }
                    }
                    else
                    {
                        result.messages.Add(Constants.DuplicateMessage);
                    }

                }
                else
                {
                    Township? data = new Township();
                    if (!isDuplicate(data, vm))
                    {
                        data = _mapper.MapViewModelToModel(data, vm);
                        result = _repository.Save(data);
                    }
                    if (result.success)
                    {
                        AuditLog(nameof(TownshipController), nameof(Township), AuditAction.CREATE.ToString());
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
            CommandResult<Township> result = new CommandResult<Township>();
            try
            {
                Township? data = _repository.Get(id);
                if (data != null)
                {
                    result = _repository.Remove(data);
                    if (result.success)
                    {
                        AuditLog(nameof(TownshipController), nameof(Township), AuditAction.DELETE.ToString());
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
            TownshipViewModel vm = new TownshipViewModel();
            try
            {
                Township? data = _repository.Get(id);
                vm = _mapper.MapModelToViewModel(data, vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(vm);
        }

        protected bool isDuplicate(Township data, TownshipViewModel vm)
        {
            bool duplicate = false;
            if (data.id > 0)
            {
                if (vm.name == data.name  && vm.country_id == data.country_id && vm.state_division_id == data.state_division_id && vm.district_id == data.district_id)
                {
                    duplicate = false;

                }
                else
                {
                    Township? dc = _repository.FindByName(vm.name);
                    if (dc != null)
                    {
                        duplicate = true;
                    }
                }
            }
            else
            {
                Township? dc = _repository.FindByName(vm.name);
                if (dc != null && dc.name.Trim() == vm.name.Trim() && dc.country_id == vm.country_id && dc.state_division_id == vm.state_division_id && dc.district_id == vm.district_id)
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
            PagedResult<TownshipViewModel> list = GetAllData();
            const string contentType = "application/octet-stream";
            HttpContext.Response.ContentType = contentType;
            HttpContext.Response.Headers.Add("attachment", "Content-Disposition");
            NPOISimpleExcelTable excel = new NPOISimpleExcelTable("Pyidaungsu", 13);
            excel.AddHeader("သဘာဝဘေးအန္တရာယ်စာရင်း");
            excel.AddColumn("စဉ်", typeof(string), NPOIExcelColumnWidth.S2);
            excel.AddColumn("နိုင်ငံအမျိုးအစား", typeof(string), NPOIExcelColumnWidth.M1);
            excel.AddColumn("နိုင်ငံ", typeof(string), NPOIExcelColumnWidth.M1);
            excel.AddColumn("တိုင်းဒေသကြီး/ပြည်နယ်", typeof(string), NPOIExcelColumnWidth.M1);
            excel.AddColumn("ခရိုင်", typeof(string), NPOIExcelColumnWidth.M1);
            excel.AddColumn("မြို့နယ်", typeof(string), NPOIExcelColumnWidth.M1);
            int count = 0;
            foreach (var item in list.data)
            {
                count++;
                excel.AddRow();
                excel.SetData(0, MyanmarEnglishConverter.ToMyanmarNumber(count.ToString()));
                excel.SetData(1, item.countryType_name);
                excel.SetData(2, item.country_name);
                excel.SetData(3, item.state_division_name);
                excel.SetData(4, item.district_name);
                excel.SetData(5, item.name);



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
