using DIS.Web.Controllers.Common;
using DIS.Web.Mappers;
using DIS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using DIS.Infrastructure.Utilities;
using DIS.DataAccess.Entity;
using DIS.DataAccess.Interfaces;

namespace DIS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramCodeController : BaseController
    {
        IProgramCodeRepository _proRepo;
        ProgramCodeMapper mapper;
        IRoleRepository _roleRepository;
        public ProgramCodeController(IRoleRepository roleRepository, IProgramCodeRepository programCodeRepository)
            : base(typeof(ProgramCodeController))
        {
            _proRepo = programCodeRepository;
            mapper = new ProgramCodeMapper();
            _roleRepository = roleRepository;

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
    }
}