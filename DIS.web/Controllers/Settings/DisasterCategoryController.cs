using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Web.Controllers.Common;
using DIS.Web.Mappers.Setttings;
using DIS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System.Linq.Expressions;

namespace DIS.Web.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]

    public class DisasterCategoryController : BaseController
    {
        IDisasterCategoryRepository _repository;
        DisasterCategoryMapper _mapper;
        public DisasterCategoryController(IDisasterCategoryRepository repository)
           : base(typeof(DisasterCategoryController))
        {
            _repository = repository;
            _mapper = new DisasterCategoryMapper();
        }

        [HttpPost]
        [Route("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(DisasterCategoryViewModel vm)
        {
            CommandResult<DisasterCategory> result = new CommandResult<DisasterCategory>();
            try
            {
                if (vm.id > 0)
                {

                }
                else
                {
                    DisasterCategory? data = new DisasterCategory();
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
    }
}
