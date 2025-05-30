using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.DataAccess.Repositories.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Infrastruture.Utilities;
using DIS.Web.Controllers.Common;
using DIS.Web.ViewModels;
using DMS.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DIS.Web.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DistrictController : BaseController
    {
        IDistrictRepository districtRepository;
        Icountry_countrytypeRepository _cctrepo;
      
        DistrictMapper _mapper;
        public DistrictController(IDistrictRepository type, Icountry_countrytypeRepository cctrepo) : base(typeof(DistrictController))
        {
            districtRepository = type;
            _mapper = new DistrictMapper();
            _cctrepo = cctrepo;

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
            PagedResult<DistrictViewModel> vmList = _mapper.MapModelToListViewModel(list,_cctrepo, districtRepository);
            return vmList;

        }
        private DistrictViewModel GetRequestParameter()
        {
            DistrictViewModel vm = new DistrictViewModel();
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
                    if (!isDuplicate(data, vm))
                    {
                        data = _mapper.MapViewModelToModel(data, vm);
                        result = districtRepository.Save(data);
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
                if (vm.name == data.name && vm.country_id ==data.country_id && vm.state_division_id==data.state_division_id)
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
                if (dc != null && dc.name.Trim() == vm.name.Trim() && dc.country_id == vm.country_id && dc.state_division_id == vm.state_division_id)
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
            PagedResult<DistrictViewModel> list = GetAllData();
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
            int count = 0;
            foreach (var item in list.data)
            {
                count++;
                excel.AddRow();
                excel.SetData(0, MyanmarEnglishConverter.ToMyanmarNumber(count.ToString()));
                excel.SetData(1, item.countryType_name);
                excel.SetData(2, item.country_name);
                excel.SetData(3, item.state_division_name);
                excel.SetData(4, item.name);



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
