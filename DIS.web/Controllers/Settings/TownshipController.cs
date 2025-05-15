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

namespace DIS.Web.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    public class TownshipController : BaseController
    {
        ITownshipRepository _repository;
        TownshipMapper _mapper;
        public TownshipController(ITownshipRepository townshipRepository) : base(typeof(TownshipController))
        {
            _repository = townshipRepository;
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
            PagedResult<TownshipViewModel> vmList = _mapper.MapModelToListViewModel(list);
            return vmList;

        }
        private TownshipViewModel GetRequestParameter()
        {
            TownshipViewModel vm = new TownshipViewModel();
            vm.name = GetRequestParameter<string>("search[name]");
            vm.country_type_id = GetRequestParameter<int>("search[country_type_id]");
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
            CommandResult<Township> result = new CommandResult<Township>();
            try
            {
                Township? data = _repository.Get(id);
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
                if (vm.name == data.name)
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
                if (dc != null)
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
                excel.SetData(1, item.country_type_name);
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
