using DIS.DataAccess.Entity;
using DIS.DataAccess.Interfaces;
using DIS.Infrastructure.Utilities;
using DIS.Web.Controllers.Common;
using DIS.Web.Mappers;
using DIS.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DIS.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : BaseController
    {
        ILogRepository _logRepo;
        LogMapper mapper;
        public LogController(ILogRepository logRepository)
            : base(typeof(LogController))
        {
            _logRepo = logRepository;
            mapper = new LogMapper();
        }
        [HttpGet]
      
        public JsonResult Get()
        {
            PagedResult<LogViewModel> list = new PagedResult<LogViewModel>();
            try
            {
                list = GetAllData();
            }
            catch (Exception ex)
            {
                list.success = false;
                list.messages.Add(ex.Message);
            }
            return Json(list);
        }

        private PagedResult<LogViewModel> GetAllData()
        {
            QueryOptions<Log> queryoption = GetQueryOptions<Log>();
            LogViewModel vm = GetRequestParameters();
            queryoption = mapper.PrepareQueryOptionForRepository(queryoption, vm);
            PagedResult<Log> list = _logRepo.GetPagedResults(queryoption);
            PagedResult<LogViewModel> vmList = mapper.MapModelToListViewMode(list);
            return vmList;
        }
        private LogViewModel GetRequestParameters()
        {
            LogViewModel vm = new LogViewModel();
            vm.url = Request.Query["url"].ToString();
            vm.action = Request.Query["action"].ToString();
            vm.user_name = Request.Query["name"].ToString();
            return vm;
        }

    }
}
