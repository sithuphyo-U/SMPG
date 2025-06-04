using DIS.DataAccess.Entity;
using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces;
using DIS.DataAccess.Interfaces.Settings;
using DIS.DataAccess.Repositories.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Infrastruture.Enumerations;
using DIS.Web.Controllers.Common;
using DIS.Web.Mappers;
using DIS.Web.Mappers.Setttings;
using DIS.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace DIS.Web.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class LabelController : BaseController
    {
        ILabelRepository _repo;
        IProgramCodeRepository _programCodeRepository;
        LabelMapper _mapper;
        public LabelController(ILabelRepository repo, IProgramCodeRepository programcodeRepo) : base(typeof(LabelController))
        {
            _repo = repo;
            _programCodeRepository = programcodeRepo;
            _mapper = new LabelMapper();
        }

        [HttpGet]
        public JsonResult Get()
        {

            List<LabelViewModel> vmList = new List<LabelViewModel>();
            try
            {
                List<Label> labelList = _repo.Get().Where(x => x.deleted == false).ToList();
                vmList = _mapper.MapModelToListViewModel(labelList);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(vmList);

        }


        [HttpPost]
        [Route("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(LabelViewModel vm)
        {
            CommandResult<Label> result = new CommandResult<Label>();
            try
            {
                if (vm.id > 0)
                {
                    Label? data = _repo.Get(vm.id);

                    data = _mapper.MapViewModelToModel(data, vm);
                    result = _repo.Save(data);
                    if (result.success)
                    {
                      
                        var parentProgramCode = _programCodeRepository.Get().FirstOrDefault(x => x.program_code == "Settings");
                        if (parentProgramCode != null)
                        {
                            var childProgramCodes = _programCodeRepository.Get()
                             .Where(x => x.parent_id == parentProgramCode.id).ToList();

                            for (int i = 0; i < childProgramCodes.Count; i++)
                            {
                                var program = childProgramCodes[i];

                                switch (i)
                                {
                                    case 0:
                                        program.program_name = vm.category_name;
                                        break;
                                    case 1:
                                        program.program_name = vm.sub_category;
                                        break;
                                    case 2:
                                        program.program_name = vm.country_type;
                                        break;
                                    case 3:
                                        program.program_name = vm.country;
                                        break;
                                    case 4:
                                        program.program_name = vm.statedivison;
                                        break;
                                    case 5:
                                        program.program_name = vm.district;
                                        break;
                                    case 6:
                                        program.program_name = vm.township;
                                        break;
                                 
                                    default:
                                        break;
                                }

                             

                                _programCodeRepository.Save(program);
                            }
                        }

                    }



                }
                else
                {
                    Label? data = new Label();

                    data = _mapper.MapViewModelToModel(data, vm);
                    result = _repo.Save(data);


                }

            }
            catch (Exception ex)
            {
                result.success = false;
                result.messages.Add(ex.Message);
                logger.LogError(ex.Message);

            }
            return Json(result);
        }


    }
}
