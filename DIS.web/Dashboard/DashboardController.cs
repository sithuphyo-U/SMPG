using DIS.Web.Controllers.Common;
using Microsoft.AspNetCore.Mvc;

namespace DIS.Web.Dashboard
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : BaseController
    {
        
        public DashboardController(Type type) : base(type)
        {
        }
    }
}
