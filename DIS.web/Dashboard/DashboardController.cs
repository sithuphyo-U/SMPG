using DIS.DataAccess.Interfaces;
using DIS.Web.Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DIS.Web.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController
    {
        IDIInfoByYearRangeDashboardRepository _yearRangeRepository;

        public DashboardController(IDIInfoByYearRangeDashboardRepository yearRangeRepository) : base(typeof(DashboardController))
        {
            _yearRangeRepository = yearRangeRepository;
        }

        [HttpGet]
        [Route("GetDisasterInfoByYearRange/")]
        public IActionResult GetDisasterInfoByYearRange(int year)
        {
            var reports = _yearRangeRepository.GetInfoByYearRange(year);
            Console.WriteLine(JsonConvert.SerializeObject(reports));
            return Json(reports);
        }
    }
}
