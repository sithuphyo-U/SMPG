using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Web.Controllers.Common;
using Microsoft.AspNetCore.Mvc;

namespace DIS.Web.Controllers.DropDown
{
    [Route("api/[controller]")]
    [ApiController]

    public class DropDownController : BaseController
    {
        IDisasterCategoryRepository _disasterCategoryRepository;

        public DropDownController(IDisasterCategoryRepository disasterCategoryRepository)

            : base(typeof(DropDownController))
        {
            _disasterCategoryRepository = disasterCategoryRepository;
        }

        [HttpGet]
        [Route("GetDisasterCategoryList")]

        public JsonResult GetDisasterCategoryList()
        {
            List<DisasterCategory> disasterCategoryList = new List<DisasterCategory>();
            try
            {
                disasterCategoryList = _disasterCategoryRepository.Get();
            }
            catch (Exception ex)
            {

            }
            return Json(disasterCategoryList);
        }

    }
}
