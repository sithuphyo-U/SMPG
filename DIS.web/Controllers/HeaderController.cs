using DIS.DataAccess.Interfaces.Settings;

     
using DIS.DataAccess.Interfaces.Settings;
using DIS.Web.Controllers.Common;
using DIS.Web.Mappers.Setttings;
using Microsoft.AspNetCore.Mvc;
using DIS.DataAccess.Entity;
using DIS.Web.ViewModels;
using DIS.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using DIS.DataAccess.Interfaces;

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
                    logger.LogError(ex, "Error while fetching header list.");
                }

                return Json(vmList);
            }

            [HttpPost("SaveOrUpdate")]
            public IActionResult SaveOrUpdate(HeaderViewModel vm)
            {
                var result = new CommandResult<Header>();

                try
                {
                    if (vm == null || string.IsNullOrWhiteSpace(vm.header_name))
                    {
                        result.success = false;
                        result.messages.Add("Invalid header data.");
                        return Json(result);
                    }

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
                        var newHeader = new Header();
                        newHeader = _mapper.MapViewModelToModel(newHeader, vm);
                        result = _headerRepository.Save(newHeader);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error while saving header.");
                    result.success = false;
                    result.messages.Add(ex.Message);
                }

                return Json(result);
            }
        }
    }
}
