using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;

using DIS.DataAccess.Entity;
using DIS.DataAccess.Interfaces;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Web.Controllers.Common;
using DIS.Web.Mappers.Setttings;
using DIS.Web.ViewModels;

namespace DIS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize] 
    public class HeaderController : BaseController
    {
        private readonly IHeaderRepository _headerRepository;
        private readonly HeaderMapper _mapper;

        public HeaderController(IHeaderRepository headerRepository)
            : base(typeof(HeaderController))
        {
            _headerRepository = headerRepository;
            _mapper = new HeaderMapper();
        }

        
        [HttpGet]
        public JsonResult Get()
        {
            var vmList = new List<HeaderViewModel>();

            try
            {
                var headers = _headerRepository.Get().Where(x => !x.deleted).ToList();
                vmList = _mapper.MapModelToListViewModel(headers);
            }
            catch (Exception ex)
            {
                logger.LogError($"[HeaderController:Get] {ex.Message}");
            }

            return Json(vmList);
        }

        
        [HttpPost("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(HeaderViewModel vm)
        {
            var result = new CommandResult<Header>();

            if (vm == null || string.IsNullOrWhiteSpace(vm.header_name))
            {
                result.success = false;
                result.messages.Add("Invalid header data.");
                return Json(result);
            }

            try
            {
                if (vm.id > 0)
                {
                    var existing = _headerRepository.Get(vm.id);
                    if (existing == null)
                    {
                        result.success = false;
                        result.messages.Add("Header not found.");
                        return Json(result);
                    }

                    existing = _mapper.MapViewModelToModel(existing, vm);
                    result = _headerRepository.Save(existing);
                }
                else
                {
                    var newHeader = _mapper.MapViewModelToModel(new Header(), vm);
                    result = _headerRepository.Save(newHeader);
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"[HeaderController:SaveOrUpdate] {ex.Message}");
                result.success = false;
                result.messages.Add("An error occurred while saving the header.");
            }

            return Json(result);
        }
    }
}
