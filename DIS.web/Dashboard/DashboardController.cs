////using DIS.DataAccess.Interfaces;
////using DIS.Web.Controllers.Common;
////using Microsoft.AspNetCore.Mvc;
////using Newtonsoft.Json;

//<<<<<<< HEAD
////namespace DIS.Web.Dashboard
////{
////    [Route("api/[controller]")]
////    [ApiController]
////    public class DashboardController : BaseController
////    {   
////        IDIInfoByYearRangeDashboardRepository _yearRangeRepository;
        
////        public DashboardController(IDIInfoByYearRangeDashboardRepository yearRangeRepository) : base(typeof(DashboardController))
////        {
////            _yearRangeRepository = yearRangeRepository;
////        }
//=======
//namespace DIS.Web.Dashboard
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class DashboardController : BaseController
//    {
//        IDIInfoByYearRangeDashboardRepository _yearRangeRepository;

//        public DashboardController(IDIInfoByYearRangeDashboardRepository yearRangeRepository) : base(typeof(DashboardController))
//        {
//            _yearRangeRepository = yearRangeRepository;
//        }
//>>>>>>> d9e453817d4ff3a8afa6be3cdea31e4b8680f91e

////        [HttpGet]
////        [Route("GetDisasterInfoByYearRange/")]
////        public IActionResult GetDisasterInfoByYearRange(int year)
////        {
////            var reports = _yearRangeRepository.GetInfoByYearRange(year);
////            Console.WriteLine(JsonConvert.SerializeObject(reports));
////            return Json(reports);
////        }
////    }
////}
