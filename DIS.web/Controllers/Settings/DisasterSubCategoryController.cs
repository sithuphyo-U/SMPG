
using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
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
            vm.name = GetRequestParameter<string>("search[name]");
            vm.category_id = GetRequestParameter<int>("search[category_id]");
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
                    if (!isDuplicate(data, vm))
                    {
                        data = _mapper.MapViewModelToModel(data, vm);
                        result = _repository.Save(data);
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
        [HttpGet]
        [Route("ExportExcel")]
        public IActionResult ExportExcel()
        {
            PagedResult<DisasterSubCategoryViewModel> list = GetAllData();
            const string contentType = "application/octet-stream";
            HttpContext.Response.ContentType = contentType;
            HttpContext.Response.Headers.Add("attachment", "Content-Disposition");
            NPOISimpleExcelTable excel = new NPOISimpleExcelTable("Pyidaungsu", 13);
            excel.AddHeader("သဘာဝဘေးအန္တရာယ်စာရင်း");
            excel.AddColumn("စဉ်", typeof(string), NPOIExcelColumnWidth.S2);
            excel.AddColumn("အမျိူးအစား", typeof(string), NPOIExcelColumnWidth.M1);
            excel.AddColumn("အမျိူးအစားခွဲ", typeof(string), NPOIExcelColumnWidth.M1);
            int count = 0;
            foreach (var item in list.data)
            {
                count++;
                excel.AddRow();
                excel.SetData(0,MyanmarEnglishConverter.ToMyanmarNumber(count.ToString()));
                excel.SetData(1,item.category_name);
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
