using DIS.DataAccess.Interfaces;
using DIS.DataAccess.Interfaces.Dashboard;
using DIS.Web.Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DIS.Web.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController
    {
        ICategoryCardDashboardRepository _ccdrepo;
        IRecentDisasterLogDashboardRepository _recentrepo;
        IDIInfoByYearRangeDashboardRepository _yearRangeRepository;
        public DashboardController(ICategoryCardDashboardRepository categoryCardDashboardRepository, IRecentDisasterLogDashboardRepository recentrepo, IDIInfoByYearRangeDashboardRepository yearRangeRepository) : base(typeof(DashboardController))
        {
            _ccdrepo = categoryCardDashboardRepository;
            _recentrepo = recentrepo;
            _yearRangeRepository = yearRangeRepository;
        }

        [HttpGet]
        [Route("GetDisasterCounts")]
        public async Task<IActionResult> GetDisasterCounts()
        {
            var data = await _ccdrepo.GetDisasterCountAsync();
            return Ok(data);
        }


        [HttpGet]
        [Route("GetRecentDisasterLogs")]
        public async Task<IActionResult> GetRecentDisasterLogs()
        {
            var data = await _recentrepo.GetRecentDisasterLogsAsync();
            return Ok(data);
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
