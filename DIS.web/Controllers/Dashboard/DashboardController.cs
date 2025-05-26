using DIS.DataAccess.Interfaces.Dashboard;
using DIS.Web.Controllers.Common;
using Microsoft.AspNetCore.Mvc;

namespace DIS.Web.Controllers.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController
    {
        ICategoryCardDashboardRepository _ccdrepo;
        IRecentDisasterLogDashboardRepository _recentrepo;
        public DashboardController(ICategoryCardDashboardRepository categoryCardDashboardRepository, IRecentDisasterLogDashboardRepository recentrepo) : base(typeof(DashboardController))
        {
            _ccdrepo = categoryCardDashboardRepository;
            _recentrepo = recentrepo;
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
    }
}
