using DIS.DataAccess.Entity;
using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Web.Controllers.Common;
using DIS.Web.ViewModels;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DIS.Web.Controllers.DropDown
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]

    public class DropDownController : BaseController
    {
        IDisasterCategoryRepository _disasterCategoryRepository;
        IDisasterSubCategoryRepository _subCategoryRepository;
        ICountryTypeRepository countryTypeRepository;
        ICountryRepository countryRepository;
        IStateDivisionRepository stateDivisionRepository;
        IDistrictRepository districtRepository;
        ITownshipRepository townshipRepository;
        IRoleRepository _roleRepo;
        ICountryTypeRepository _countryTypeRepository;
        ICountryRepository _countryRepository;
        IStateDivisionRepository _stateDivisionRepository;
        IDistrictRepository _districtRepository;
        ITownshipRepository _townshipRepository;
        Icountry_countrytypeRepository _cctrepo;

        public DropDownController(IDisasterCategoryRepository disasterCategoryRepository, IDisasterSubCategoryRepository subCategoryRepository, ICountryTypeRepository countryTypeRepository, ICountryRepository countryRepository, IStateDivisionRepository stateDivisionRepository, IDistrictRepository districtRepository, ITownshipRepository townshipRepository, IRoleRepository roleRepository, Icountry_countrytypeRepository cctrepo)

            : base(typeof(DropDownController))
        {
            _disasterCategoryRepository = disasterCategoryRepository;
            _subCategoryRepository = subCategoryRepository;
            countryTypeRepository = countryTypeRepository;
            countryRepository = countryRepository;
            stateDivisionRepository = stateDivisionRepository;
            districtRepository = districtRepository;
            townshipRepository = townshipRepository;
            _roleRepo = roleRepository;
            _countryTypeRepository = countryTypeRepository;
            _countryRepository = countryRepository;
            _stateDivisionRepository = stateDivisionRepository;
            _districtRepository = districtRepository;
            _townshipRepository = townshipRepository;
            _cctrepo = cctrepo;
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
                countrytypelist = _countryTypeRepository.Get();
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
                countrylist = _countryRepository.Get();
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
                statedivisionlist = _stateDivisionRepository.Get();
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
                districtlist = _districtRepository.Get();
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
                townshiplist = _townshipRepository.Get();
            }
            catch (Exception ex)
            {

            }
            return Json(townshiplist);
        }
        [HttpGet]
        [Route("GetRoleList")]

        public JsonResult GetRoleList()
        {
            List<Role> rolelist = new List<Role>();
            try
            {
                rolelist = _roleRepo.Get();
            }
            catch (Exception ex)
            {

            }
            return Json(rolelist);
        }
        [HttpGet]
        [Route("GetSubCategoryById")]
        public JsonResult GetSubCategoryById(int id)
        {
            List<DisasterSubCategory> subdata = _subCategoryRepository.GetSubCategorybyCategory(id);
            return Json(subdata);
        }

        [HttpGet]
        [Route("GetCountryById")]
        public JsonResult GetCountryById(int id)
        {
           // List<Country>? GetCountrybyCountryType(int id);
            List<country_countrytype> cctlist = _cctrepo.GetByCountryTypeByCountryId(id);
            CountryViewModel cc = new CountryViewModel();
            foreach (var c in cctlist)
            {

                cc.name = c.Country.name;
            }
            cc.country_type_name.Add(cc.name);
            return Json(cctlist);
        }


        [HttpGet]
        [Route("GetStateDivisionById")]
        public JsonResult GetStateDivisionById(int id)
        {
            List<StateDivision> subdata = _stateDivisionRepository.GetStateDivisionbyCountry(id);
            return Json(subdata);
        }


        [HttpGet]
        [Route("GetDistrictById")]
        public JsonResult GetDistrictById(int id)
        {
            List<District> subdata = _districtRepository.GetDistrictByStateDivision(id);
            return Json(subdata);
        }


        [HttpGet]
        [Route("GetTownshipById")]
        public JsonResult GetTownshipById(int id)
        {
            List<Township> subdata = _townshipRepository.GetTownshipByDistrict(id);
            return Json(subdata);
        }


        

    }
}
