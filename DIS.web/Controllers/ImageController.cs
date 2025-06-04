using DIS.DataAccess.Interfaces.Settings;
using DIS.DataAccess.Interfaces;
using DIS.Web.Controllers.Common;
using DIS.Web.Controllers.Settings;
using DIS.Web.Mappers.Setttings;
using NPOI.SS.Formula.Functions;
using DIS.DataAccess.Repositories;
using DIS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using DIS.DataAccess.Entity;
using DIS.Web.Mappers;
using DIS.Infrastructure.Utilities;
using DIS.Application.Service;
using DIS.Application.Services;
using DIS.Application.Services.Common;
using Microsoft.AspNetCore.Authorization;

namespace DIS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ImageController : BaseController
    {
        IImageRepository _imageRepository;
        ImageMapper _mapper;
        ImageFileService _imageFileService;
        FileService fileService;



        public ImageController(IImageRepository imageRepository, ImageFileService imageFileService, FileService _fileService) : base(typeof(LabelController))
        {
            _imageRepository = imageRepository;
            _mapper = new ImageMapper();
            _imageFileService = imageFileService;
            fileService = _fileService;
        }


        [HttpGet]
        public JsonResult Get()
        {

            List<ImageViewModel> vmList = new List<ImageViewModel>();
            try
            {
                List<Image> labelList = _imageRepository.Get().Where(x => x.deleted == false).ToList();
                vmList = _mapper.MapModelToListViewModel(labelList);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(vmList);

        }

        [HttpPost]
        [Route("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(ImageViewModel vm)
        {
            CommandResult<Image> result = new CommandResult<Image>();
            try
            {
                if (vm.imageId > 0)
                {
                    Image oldFiles = _imageRepository.Get(vm.imageId);


                    string incomingFileName = vm.form_files?.FileName;


                    if (!incomingFileName.Contains(oldFiles.original_image_name))
                    {
                        var path = Path.Combine(Constants.FilePath + "/IconImage", oldFiles.image_name);
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        _imageFileService.Delete(oldFiles);
                    }
                    if (vm.form_files != null)
                    {
                        Image? entity = new Image();
                        IFormFile? file = vm.form_files;
                        Guid guId = Guid.NewGuid();
                        string extension = Path.GetExtension(file.FileName).ToLower();
                        string path = "/IconImage/";
                        entity.image_name = guId.ToString();
                        entity.image_type = extension;
                        entity.original_image_name = file.FileName;
                        entity.path = path;
                        var returndata = _imageFileService.SaveorUpdate(entity);
                        if (returndata != null)
                        {
                            fileService.CreatedPhysicalFile(Constants.FilePath + entity.path, entity.image_name, file);
                        }
                    }
                }



                else
                {
                    Image? data = new Image();
                    IFormFile? file = vm.form_files;
                    Guid guId = Guid.NewGuid();
                    string extension = Path.GetExtension(file.FileName).ToLower();
                    string path = "/IconImage/";
                    data.image_name = guId.ToString();
                    data.image_type = extension;
                    data.original_image_name = file.FileName;
                    data.path = path;
                    var returndata = _imageFileService.SaveorUpdate(data);
                    if (returndata != null)
                    {
                        fileService.CreatedPhysicalFile(Constants.FilePath + data.path, data.image_name, file);
                    }


                }

            }
            catch (Exception e)
            {
            }
            return Json(result);
        }

        [HttpGet("view-image/{fileName}")]
        public IActionResult ViewImage(string fileName)
        {
            var rootPath = Path.Combine(Constants.FilePath, "IconImage");


            var filePath = Directory.GetFiles(rootPath, fileName, SearchOption.AllDirectories)
                                    .FirstOrDefault();

            if (filePath == null || !System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var contentType = "image/jpeg";
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType);
        }
    }
}
