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
using NPOI.HPSF;
using Microsoft.IdentityModel.Tokens;
using DocumentFormat.OpenXml.Packaging;
using System.Text;
using UglyToad.PdfPig;
using System.Text.RegularExpressions;
using DIS.Application.Service.Common;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using Microsoft.AspNetCore.Authorization;

namespace DIS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

  //  [Authorize]

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
            PagedResult<DisasterInfoViewModel> list = new PagedResult<DisasterInfoViewModel>();
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
            PagedResult<DisasterInfoViewModel> vmList;

            // Apply filter and remap
            if (!vm.word.IsNullOrEmpty())
            {
                var filteredList = FilterByWord(list.data, vm.word);

                vmList = new PagedResult<DisasterInfoViewModel>
                {
                    data = filteredList,
                    total = filteredList.Count,

                };
            }
            else
            {
                // No filter applied, use normal mapping
                vmList = _mapper.MapModelToListViewModel(list, _disasterInfoFileRepo);
            }

            return vmList;


        }


        private DisasterInfoViewModel GetRequestParameter()
        {
            DisasterInfoViewModel vm = new DisasterInfoViewModel();
            vm.from_date = GetRequestParameter<string>("search[from_date]");
            vm.to_date = GetRequestParameter<string>("search[to_date]");
            vm.title = GetRequestParameter<string>("search[title]");
            vm.country_type_id = GetRequestParameter<int>("search[country_type_id]");
            vm.country_id = GetRequestParameter<int>("search[country_id]");
            vm.state_division_id = GetRequestParameter<int>("search[state_division_id]");
            vm.district_id = GetRequestParameter<int>("search[district_id]");
            vm.disaster_category_id = GetRequestParameter<int>("search[disasterCategory_id]");
            vm.subcategory_id = GetRequestParameter<int>("search[subcategories_id]");
            vm.word = GetRequestParameter<string>("search[word]");

            return vm;
        }
        //[HttpGet]
        //[Route("filter")]
        //public List<DisasterInfoViewModel> FilterByWord(List<DisasterInfo> list, string word)
        //{
        //    var filtered = new List<DisasterInfoViewModel>();
        //    string orderedInput = string.Concat(word.OrderBy(c => c));

        //    foreach (var item in list)
        //    {
        //        int wordCount = 0;
        //        var matchedFiles = new List<File_TB>();

        //        // Fetch both .docx and .pdf files
        //        var files = _disasterInfoFileRepo.GetDataById(item.id)
        //                      .Where(f => f.file_type == ".docx" || f.file_type == ".pdf");

        //        foreach (var file in files)
        //        {
        //            string fullPath = Path.Combine(Constants.FilePath, "DisasterInfoFile", file.file_name);

        //            if (System.IO.File.Exists(fullPath))
        //            {
        //                string content = string.Empty;


        //                if (file.file_type == ".docx")
        //                {
        //                    using (WordprocessingDocument doc = WordprocessingDocument.Open(fullPath, false))
        //                    {
        //                        content = doc.MainDocumentPart.Document.Body.InnerText;
        //                    }
        //                }
        //                else if (file.file_type == ".pdf")
        //                {
        //                    // Extract PDF text here
        //                    var result = ExtractTextFromPdf(fullPath, word);

        //                    content = result.filedata;
        //                    wordCount = result.count;

        //                }

        //                if (!string.IsNullOrEmpty(content))
        //                {
        //                    if (content.Contains(word, StringComparison.OrdinalIgnoreCase) ||
        //                        string.Concat(content.Where(char.IsLetter).OrderBy(c => c)).Contains(orderedInput))
        //                    {
        //                        matchedFiles.Add(file);

        //                    }
        //                }
        //            }
        //        }

        //        if (matchedFiles.Any())
        //        {
        //            var vm = _mapper.MapModelToViewModel(item, new DisasterInfoViewModel());
        //            vm.FilteredFiles = matchedFiles;
        //            vm.count=wordCount;  
        //            filtered.Add(vm);
        //        }
        //    }

        //    return filtered;
        //}

        [HttpGet]
        [Route("filter")]
        public List<DisasterInfoViewModel> FilterByWord(List<DisasterInfo> list, string word)
        {
            var filtered = new List<DisasterInfoViewModel>();
            string orderedInput = string.Concat(word.OrderBy(c => c));

            foreach (var item in list)
            {
                var matchedFiles = new List<FilteredFileViewModel>();

                var files = _disasterInfoFileRepo.GetDataById(item.id)
                              .Where(f => f.file_type == ".docx" || f.file_type == ".pdf");

                foreach (var file in files)
                {
                    string fullPath = Path.Combine(Constants.FilePath, "DisasterInfoFile", file.file_name);

                    if (System.IO.File.Exists(fullPath))
                    {
                        string content = string.Empty;
                        int wordCount = 0;

                        if (file.file_type == ".docx")
                        {
                            using (WordprocessingDocument doc = WordprocessingDocument.Open(fullPath, false))
                            {
                                content = doc.MainDocumentPart.Document.Body.InnerText;
                                wordCount = Regex.Matches(content, Regex.Escape(word), RegexOptions.IgnoreCase).Count;
                            }
                        }
                        else if (file.file_type == ".pdf")
                        {
                            var result = ExtractTextFromPdf(fullPath, word);
                            content = result.filedata;
                            wordCount = result.count;
                        }

                        if (!string.IsNullOrEmpty(content))
                        {
                            if (content.Contains(word, StringComparison.OrdinalIgnoreCase) ||
                                string.Concat(content.Where(char.IsLetter).OrderBy(c => c)).Contains(orderedInput))
                            {
                                matchedFiles.Add(new FilteredFileViewModel
                                {
                                    File = file,
                                    WordCount = wordCount
                                });
                            }
                        }
                    }
                }

                if (matchedFiles.Any())
                {
                    var vm = _mapper.MapModelToViewModel(item, new DisasterInfoViewModel(), _TBRepository);
                    vm.FilteredFiles = matchedFiles;
                    filtered.Add(vm);
                }
            }

            return filtered;
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
                            
                            List<File_TB> oldFiles = _TBRepository.GetFilebyDisasterInfoId(vm.id);

                           
                            List<string> incomingFileNames = vm.file_list?.Select(f => f.FileName).ToList() ?? new List<string>();

                        
                            foreach (var oldFile in oldFiles)
                            {
                                if (!incomingFileNames.Contains(oldFile.originalfile_name))
                                {
                                    var path = Path.Combine(Constants.FilePath + "/DisasterInfoFile", oldFile.file_name);
                                    if (System.IO.File.Exists(path))
                                    {
                                        System.IO.File.Delete(path);
                                    }
                                    _dsInfoFileService.Delete(oldFile);
                                }
                            }

                            // 4. Save only new files
                            foreach (var f in vm.file_list)
                            {
                               
                                bool exists = oldFiles.Any(of => of.originalfile_name == f.FileName);
                                if (!exists)
                                {
                                    string extension = Path.GetExtension(f.FileName).ToLower();
                                    if (extension == ".pdf" || extension == ".docx" || extension == ".jpg" || extension == ".png" || extension == ".mp3" || extension == ".mp4")
                                    {
                                        Guid guId = Guid.NewGuid();
                                        File_TB entity = new File_TB
                                        {
                                            file_name = guId.ToString(),
                                            file_type = extension,
                                            originalfile_name = f.FileName,
                                            disastercategory_id = data.id,
                                            path = "/DisasterInfoFile"
                                        };

                                        var returndata = _dsInfoFileService.SaveorUpdate(entity);
                                        if (returndata != null)
                                        {
                                            fileService.CreatedPhysicalFile(Constants.FilePath + entity.path, entity.file_name, f);
                                        }
                                    }
                                }
                            }
                        }

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
                                string extension = Path.GetExtension(f.FileName).ToLower();
                                if (extension == ".pdf" || extension == ".docx" || extension == ".jpg" || extension == ".png" || extension == ".mp3" || extension == ".mp4")
                                {
                                    Guid guId = Guid.NewGuid();
                                    File_TB entity = new File_TB();
                                    entity.file_name = guId.ToString();
                                    entity.file_type = extension;
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

        [HttpGet("view-pdf/{fileName}")]
        public IActionResult ViewPdf(string fileName)
        {
            var filePath = Path.Combine(Constants.FilePath, "DisasterInfoFile", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/pdf");
        }


        [HttpGet("view-image/{fileName}")]
        public IActionResult ViewImage(string fileName)
        {
            var filePath = Path.Combine(Constants.FilePath, "DisasterInfoFile", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var contentType = "image/jpeg";
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType);
        }

        [HttpGet("view-audio/{fileName}")]
        public IActionResult ViewAudio(string fileName)
        {
            var filePath = Path.Combine(Constants.FilePath, "DisasterInfoFile", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var contentType = "audio/mpeg"; // MIME type for MP3
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType);
        }

        [HttpGet("view-video/{fileName}")]
        public IActionResult ViewVideo(string fileName)
        {
            var filePath = Path.Combine(Constants.FilePath, "DisasterInfoFile", fileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var contentType = "video/mp4"; // MIME type for MP4
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType);
        }

        [HttpGet("download-doc/{filename}")]
        public IActionResult DownloadFile(string filename)
        {
            var filePath = Path.Combine(Constants.FilePath, "DisasterInfoFile", filename);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { success = false, message = "File not found" });
            }

            var contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";


            var bytes = System.IO.File.ReadAllBytes(filePath);
            return File(bytes, contentType);
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




        [HttpGet]
        [Route("getbyid/")]
        public JsonResult GetById(int id)
        {
            DisasterInfoViewModel vm = new DisasterInfoViewModel();
            try
            {
                DisasterInfo? data = _repository.Get(id);
                vm = _mapper.MapModelToViewModel(data, vm, _TBRepository);
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
        private class Data
        {
            public string filedata { get; set; }
            public int count { get; set; }
        }
        private Data ExtractTextFromPdf(string fullPath, string searchWord)
        {
            Data data = new Data();
            int totalWordCount = 0;
            var sb = new StringBuilder();

            using (PdfDocument document = PdfDocument.Open(fullPath))
            {
                foreach (UglyToad.PdfPig.Content.Page page in document.GetPages())

                {
                    if (!string.IsNullOrEmpty(searchWord))
                    {
                        string normalizedText = page.Text.Normalize(NormalizationForm.FormKC);
                        string normalizedSearch = searchWord.Normalize(NormalizationForm.FormKC);

                        int count = Regex.Matches(normalizedText, Regex.Escape(normalizedSearch), RegexOptions.IgnoreCase).Count;

                        //int count = Regex.Matches(page.Text, Regex.Escape("Javascript"), RegexOptions.IgnoreCase).Count;
                        totalWordCount += count;
                    }

                    sb.AppendLine(page.Text);
                }
            }
            data.filedata = sb.ToString();
            data.count = totalWordCount;
            return data;
        }




    }
}

