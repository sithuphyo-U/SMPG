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
        ICountryTypeRepository countryTypeRepository;
        ICountryRepository countryRepository;
        IStateDivisionRepository stateDivisionRepository;
        IDistrictRepository districtRepository;
        ITownshipRepository townshipRepository;

        public DropDownController(IDisasterCategoryRepository disasterCategoryRepository, ICountryTypeRepository countryTypeRepository, ICountryRepository countryRepository, IStateDivisionRepository stateDivisionRepository, IDistrictRepository districtRepository, ITownshipRepository townshipRepository)

            : base(typeof(DropDownController))
        {
            _disasterCategoryRepository = disasterCategoryRepository;
            countryTypeRepository = countryTypeRepository;
          countryRepository = countryRepository;
            stateDivisionRepository = stateDivisionRepository;
            districtRepository = districtRepository;
            townshipRepository = townshipRepository;
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


        [HttpGet]
        [Route("GetCountryTypeList")]

        public JsonResult GetCountryTypeList()
        {
            List<CountryType> countrytypelist = new List<CountryType>();
            try
            {
                countrytypelist = countryTypeRepository.Get();
            }
            catch (Exception ex)
            {

            }
            return Json(countrytypelist);
        }

        [HttpGet]
        [Route("GetCountryList")]

        public JsonResult GetCountryList()
        {
            List<Country> countrylist = new List<Country>();
            try
            {
                countrylist = countryRepository.Get();
            }
            catch (Exception ex)
            {

            }
            return Json(countrylist);
        }

        [HttpGet]
        [Route("GetStateDivisionList")]

        public JsonResult GetStateDivisionList()
        {
            List<StateDivision> statedivisionlist = new List<StateDivision>();
            try
            {
                statedivisionlist = stateDivisionRepository.Get();
            }
            catch (Exception ex)
            {

            }
            return Json(statedivisionlist);
        }

        [HttpGet]
        [Route("GetDistrictList")]

        public JsonResult GetDistrictList()
        {
            List<District> districtlist = new List<District>();
            try
            {
                districtlist = districtRepository.Get();
            }
            catch (Exception ex)
            {

            }
            return Json(districtlist);
        }

        [HttpGet]
        [Route("GetTownshipList")]

        public JsonResult GetTownshipList()
        {
            List<Township> townshiplist = new List<Township>();
            try
            {
                townshiplist = townshipRepository.Get();
            }
            catch (Exception ex)
            {

            }
            return Json(townshiplist);
        }

    }
}
