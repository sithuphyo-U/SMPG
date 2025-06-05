using DIS.Web.Controllers.Common;
using DIS.Web.Mappers;
using DIS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DIS.Infrastructure.Utilities;
using DIS.DataAccess.Entity;
using DIS.DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using DIS.DataAccess.Repositories;
using NPOI.SS.Formula.Functions;
using DIS.DataAccess.Interfaces.Settings;
using DIS.DataAccess.Entity.Settings;

namespace DIS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProgramCodeController : BaseController
    {
        IProgramCodeRepository _proRepo;
        ProgramCodeMapper mapper;
        IRoleRepository _roleRepository;
        ILabelRepository _labelRepository;
        public ProgramCodeController(IRoleRepository roleRepository, IProgramCodeRepository programCodeRepository,ILabelRepository labelRepository)
            : base(typeof(ProgramCodeController))
        {
            _proRepo = programCodeRepository;
            mapper = new ProgramCodeMapper();
            _roleRepository = roleRepository;
            _labelRepository = labelRepository;

        }
        [HttpGet]
        public JsonResult Get()
        {

            List<ProgramCodeViewModel> vmList = new List<ProgramCodeViewModel>();
            try
            {
                List<ProgramCode> programCodeList = _proRepo.Get().Where(x => x.deleted == false).ToList();
                vmList = ProgramCodeMapper.MapModelToListViewModel(programCodeList, _roleRepository);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(vmList);

        }


        [HttpGet]
        [Route("get_by_role_id")]
        public JsonResult GetByRole(int id)
        {
            List<PermissionViewModel> vmList = new List<PermissionViewModel>();
            try
            {
                List<ProgramCode> programCodeList = _proRepo.Get().Where(x => x.deleted == false && x.parent_id == 0).ToList();
                vmList = ProgramCodeMapper.MapModelToPermissionViewModel(programCodeList, _roleRepository, id);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(vmList);
        }

        [HttpPost]
        [Route("save_permission")]

        public IActionResult SavePermission(List<PermissionViewModel> vmList)
        {
            CommandResult<ProgramCode> result = new CommandResult<ProgramCode>();
            try
            {
                List<ProgramCode> programCodeList = _proRepo.Get().Where(x => x.deleted == false).ToList();

                foreach (var vm in vmList)
                {
                    ProgramCode? pCode = programCodeList.Where(x => x.id == vm.program_code_id).FirstOrDefault();
                    List<Role> roleList = _roleRepository.Get().Where(x => x.deleted == false).ToList();
                    if (pCode != null)
                    {
                        List<PermissionViewModel> pVMList = new List<PermissionViewModel>();
                        if (pCode.permission != null)
                        {
                            List<PermissionViewModel>? data = JsonConvert.DeserializeObject<List<PermissionViewModel>>(pCode.permission);

                            foreach (var role in roleList)
                            {
                                PermissionViewModel? pvm = data.Where(x => x.role_id == role.id).FirstOrDefault();
                                if (pvm != null)
                                {
                                    if (role.id == vm.role_id)
                                    {
                                        pvm.read = vm.read;
                                        pvm.write = vm.write;
                                        pvm.delete = vm.delete;
                                        pVMList.Add(pvm);
                                    }
                                    else
                                    {
                                        pVMList.Add(pvm);
                                    }
                                }
                                else
                                {
                                    pvm = new PermissionViewModel();
                                    pvm.id = data.Count + 1;
                                    pvm.role_name = role.name;
                                    pvm.role_id = role.id;
                                    pvm.program_name = pCode.program_name;
                                    pvm.program_code_id = pCode.id;
                                    pvm.read = false;
                                    pvm.write = false;
                                    pvm.delete = false;
                                    pVMList.Add(pvm);
                                }
                            }
                        }
                        else
                        {
                            int count = 0;
                            foreach (var role in roleList)
                            {
                                count++;
                                PermissionViewModel? pvm = new PermissionViewModel();
                                if (role.id == vm.role_id)
                                {
                                    pvm.id = count;
                                    pvm.role_name = role.name;
                                    pvm.role_id = role.id;
                                    pvm.program_name = pCode.program_name;
                                    pvm.program_code_id = pCode.id;
                                    pvm.read = vm.read;
                                    pvm.write = vm.write;
                                    pvm.delete = vm.delete;
                                    pVMList.Add(pvm);
                                }
                                else
                                {
                                    pvm.id = count;
                                    pvm.role_name = role.name;
                                    pvm.role_id = role.id;
                                    pvm.program_name = pCode.program_name;
                                    pvm.program_code_id = pCode.id;
                                    pvm.read = false;
                                    pvm.write = false;
                                    pvm.delete = false;
                                    pVMList.Add(pvm);
                                }
                            }
                        }
                        pCode.permission = JsonConvert.SerializeObject(pVMList);
                        result = _proRepo.Save(pCode);

                    }

                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(result);
        }

        [HttpPost]
        [Route("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(ProgramNameViewModel updatedLabels)
        {
            var result = new CommandResult<ProgramCode>();
            // Retrieve parent program code
            List<ProgramCode> parentProgramCode = _proRepo.Get().Where(x => x.parent_id == 0).ToList();
            var SettingParent = _proRepo.Get().FirstOrDefault(x => x.program_code == "Settings");
            parentProgramCode[0].program_name = updatedLabels.Dashboard;
            parentProgramCode[1].program_name = updatedLabels.DisasterInfo;
            parentProgramCode[2].program_name = updatedLabels.Settings;
            parentProgramCode[3].program_name = updatedLabels.DataManagement;
            parentProgramCode[4].program_name = updatedLabels.UserManagement;
            parentProgramCode[5].program_name = updatedLabels.RoleManagement;
            parentProgramCode[6].program_name = updatedLabels.Log;
            if (SettingParent != null)
            {

                if (SettingParent != null)
                {
                    var childProgramCodes = _proRepo.Get()
                        .Where(x => x.parent_id == SettingParent.id)
                        .ToList();

                    for (int i = 0; i < childProgramCodes.Count; i++)
                    {
                        var program = childProgramCodes[i];

                        switch (i)
                        {
                            case 0:
                                program.program_name = updatedLabels.DisasterCategory;
                                break;
                            case 1:
                                program.program_name = updatedLabels.DisasterSubCategory;
                                break;
                            case 2:
                                program.program_name = updatedLabels.CountryType;
                                break;
                            case 3:
                                program.program_name = updatedLabels.Country;
                                break;
                            case 4:
                                program.program_name = updatedLabels.StateDivision;
                                break;
                            case 5:
                                program.program_name = updatedLabels.District;
                                break;
                            case 6:
                                program.program_name = updatedLabels.Township;
                                break;

                            default:
                                break;
                        }

                        
                         result =_proRepo.Save(program);
                       
                       
                    }
                    if (result.success)
                    {
                        var label = _labelRepository.Get().FirstOrDefault();
                        if (label != null)
                        {
                            label.category_name = updatedLabels.DisasterCategory;
                            label.sub_category = updatedLabels.DisasterSubCategory;
                            label.country_type = updatedLabels.CountryType;
                            label.country = updatedLabels.Country;
                            label.statedivison = updatedLabels.StateDivision;
                            label.district = updatedLabels.District;
                            label.township = updatedLabels.Township;

                            _labelRepository.Save(label);
                        }
                    }

                    foreach (var parent in parentProgramCode)
                    {
                        _proRepo.Save(parent);
                    }

                }

               
            }

            return Json("Labels updated successfully.");

        }




    }
}