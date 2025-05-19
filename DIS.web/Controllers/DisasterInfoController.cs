using DIS.Application.Service;
using DIS.Application;
using DIS.DataAccess.Entity;
using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Infrastruture.Utilities;
using DIS.Web.Controllers.Common;
using DIS.Web.Mappers;
using DIS.Web.ViewModels;
using DMS.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using DIS.Application.Services;

namespace DIS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisasterInfoController : BaseController
    {
        IDisasterInfoRepository _repository;
        ICountryTypeRepository _countrytyperepo;
        ICountryRepository _countryrepo;
        IStateDivisionRepository _statedivisionrepo;
        IDistrictRepository _districtrepo;
        ITownshipRepository _townshiprepo;
        IDisasterCategoryRepository _disastercategoryrepo;
        IDisasterSubCategoryRepository _disastersubcategoryrepo;
        IFile_TBRepository _TBRepository;
        IDisasterInfoFileRepository _disasterInfoFileRepo;
        DisasterInfoFileService _dsInfoFileService;
        DisasterInfoMapper _mapper;
        FileService fileService;

        public DisasterInfoController(ICountryTypeRepository countryTypeRepository, IDisasterInfoFileRepository disasterInfoFileRepo, FileService _fileService, IFile_TBRepository TBRepository, ICountryRepository countryRepository, IStateDivisionRepository stateDivisionRepository, IDistrictRepository districtRepository, ITownshipRepository townshipRepository, IDisasterCategoryRepository disasterCategoryRepository, IDisasterSubCategoryRepository disasterSubCategoryRepository, IDisasterInfoRepository disasterInfoRepository, DisasterInfoFileService dsInfoFileService) : base(typeof(DisasterInfoController))
        {
            _repository = disasterInfoRepository;
            _countrytyperepo = countryTypeRepository;
            _countryrepo = countryRepository;
            _statedivisionrepo = stateDivisionRepository;
            _districtrepo = districtRepository;
            _townshiprepo = townshipRepository;
            _disastercategoryrepo = disasterCategoryRepository;
            _disastersubcategoryrepo = disasterSubCategoryRepository;
            _TBRepository = TBRepository;
            _disasterInfoFileRepo = disasterInfoFileRepo;
            _mapper = new DisasterInfoMapper();
            _dsInfoFileService = dsInfoFileService;
            fileService = _fileService;
        }

        [HttpGet]
        public JsonResult Get()
        {
            PagedResult <DisasterInfoViewModel> list = new PagedResult<DisasterInfoViewModel>();
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

        private PagedResult<DisasterInfoViewModel> GetAllData()
        {

            QueryOptions<DisasterInfo> queryOptions = GetQueryOptions<DisasterInfo>();
            DisasterInfoViewModel vm = GetRequestParameter();
            queryOptions = _mapper.PrepareQueryOptionForRepository(queryOptions, vm);
            PagedResult<DisasterInfo> list = _repository.GetPagedResults(queryOptions);
            PagedResult<DisasterInfoViewModel> vmList = _mapper.MapModelToListViewModel(list, _disasterInfoFileRepo);
          
            return vmList;


        }


        private DisasterInfoViewModel GetRequestParameter()
        {
            DisasterInfoViewModel vm = new DisasterInfoViewModel();
            vm.title = GetRequestParameter<string>("search[title]");
            vm.country_type_id = GetRequestParameter<int>("search[country_type_id]");
            vm.country_id = GetRequestParameter<int>("search[country_id]");
            vm.state_division_id = GetRequestParameter<int>("search[state_division_id]");
            vm.district_id = GetRequestParameter<int>("search[district_id]");
            vm.disaster_category_id = GetRequestParameter<int>("search[disasterCategory_id]");
            vm.subcategory_id = GetRequestParameter<int>("search[subCategory_id]");
            return vm;
        }
     
    [HttpPost]
        [Route("SaveOrUpdate")]
        public IActionResult SaveOrUpdate([FromForm] DisasterInfoViewModel vm)
        {
            CommandResult<DisasterInfo> result = new CommandResult<DisasterInfo>();
            try
            {
                if (vm.id > 0)
                {
                    DisasterInfo? data = _repository.Get(vm.id);
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
                    DisasterInfo? data = new DisasterInfo();
                    
                        data = _mapper.MapViewModelToModel(data, vm);
                        result = _repository.Save(data);
                        if (result.success)
                        {
                            if (vm.file_list != null && vm.file_list.Count > 0)
                            {
                                foreach (var f in vm.file_list)
                                {
                                    if (f.ContentType == "application/pdf")
                                    {
                                    Guid guId = Guid.NewGuid();
                                    File_TB entity = new File_TB();
                                    entity.file_name = guId.ToString();
                                    entity.file_type = f.ContentType;
                                    entity.originalfile_name = f.FileName;
                                    entity.disastercategory_id = data.id;
                                   
                                    entity.path = "/DisasterInfoFile";
                                    
                                    var returndata = _dsInfoFileService.SaveorUpdate(entity);
                                    if (returndata != null)
                                    {
                                        fileService.CreatedPhysicalFile(Constants.FilePath + entity.path, entity.file_name, f);
                                    }




                                }
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
        [HttpGet("view-pdf/{fileName}")]
        public IActionResult ViewPdf(string fileName)
        {
            var filePath = Path.Combine(Constants.FilePath, "DisasterInfoFile", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf");
        }


        protected bool isDuplicate(DisasterInfo data, DisasterInfoViewModel vm)
        {
            bool duplicate = false;
            if (data.id > 0)
            {
                if (vm.title == data.title)
                {
                    duplicate = false;

                }
                else
                {
                    DisasterInfo? dc = _repository.FindByTitle(vm.title);
                    if (dc != null)
                    {
                        duplicate = true;
                    }
                }
            }
            else
            {
                DisasterInfo? dc = _repository.FindByTitle(vm.title);
                if (dc != null)
                {
                    duplicate = true;
                }
            }
            return duplicate;


        }

        [HttpDelete]
        [Route("delete/")]
        public JsonResult Delete(int id)
        {
            CommandResult<DisasterInfo> result = new CommandResult<DisasterInfo>();
            try
            {
                DisasterInfo? data = _repository.Get(id);
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
            DisasterInfoViewModel vm = new DisasterInfoViewModel();
            try
            {
                DisasterInfo? data = _repository.Get(id);
                vm = _mapper.MapModelToViewModel(data, vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(vm);
        }


        [HttpGet]
        [Route("ExportExcel")]
        public IActionResult ExportExcel()
        {
            PagedResult<DisasterInfoViewModel> list = GetAllData();
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
            excel.AddColumn("သဘာဝဘေးအန္တရာယ်အမျိုးအစား", typeof(string), NPOIExcelColumnWidth.M1);
            excel.AddColumn("သဘာဝဘေးအန္တရာယ်အမျိုးအစားခွဲအမည်", typeof(string), NPOIExcelColumnWidth.M1);
            excel.AddColumn("‌ေသတင်းခေါင်းစဉ်", typeof(string), NPOIExcelColumnWidth.M1);
            //excel.AddColumn("အကြောင်းအရာ", typeof(string), NPOIExcelColumnWidth.M1);
            //excel.AddColumn("ရက်စွဲ", typeof(string), NPOIExcelColumnWidth.M1);
            //excel.AddColumn("အချိန်", typeof(string), NPOIExcelColumnWidth.M1);
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
                excel.SetData(5, item.disasterCategory_name);
                excel.SetData(6, item.subCategory_name);
                excel.SetData(7, item.title);



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

