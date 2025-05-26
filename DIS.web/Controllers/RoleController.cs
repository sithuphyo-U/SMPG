using DIS.DataAccess.Entity;
using DIS.DataAccess.Interfaces;
using DIS.Infrastructure.Utilities;
using DIS.Web.Controllers.Common;
using DIS.Web.Mappers;
using DIS.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DIS.Web.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
   
    public class RoleController : BaseController
    {
        IRoleRepository _roleRepo;
        RoleMapper mapper;
        public RoleController(IRoleRepository roleRepository)
            : base(typeof(RoleController))
        {
            _roleRepo = roleRepository;
            mapper = new RoleMapper();
        }
        [HttpGet]
        public JsonResult Get()
        {
            PagedResult<RoleViewModel> list = new PagedResult<RoleViewModel>();
            try
            {
                list = GetAllData();
            }
            catch (Exception ex)
            {
                list.success = false;
                list.messages.Add(ex.Message);
                logger.LogError(ex.Message);
            }
            return Json(list);
        }
        private PagedResult<RoleViewModel> GetAllData()
        {
            QueryOptions<Role> queryoption = GetQueryOptions<Role>();
            RoleViewModel vm = GetRequestParameters();
            queryoption = mapper.PrepareQueryOptionForRepository(queryoption, vm);
            PagedResult<Role> list = _roleRepo.GetPagedResults(queryoption);
            PagedResult<RoleViewModel> vmList = mapper.MapModelToListViewMode(list);
            return vmList;
        }

        private RoleViewModel GetRequestParameters()
        {
            RoleViewModel vm = new RoleViewModel();
            vm.name = Request.Query["name"].ToString();
            return vm;
        }
        [HttpPost]
        [Route("SaveOrUpdate")]
        public IActionResult SaveOrUpdate(RoleViewModel vm)
        {
            CommandResult<Role> result = new CommandResult<Role>();
            try
            {
                if (vm.id > 0)
                {
                    Role? data = _roleRepo.Get(vm.id);
                    if (!isDuplicate(data, vm))
                    {
                        data = mapper.MapViewModelToModel(data, vm);
                        data.modified_by = GetLoggedInUserId();
                        result = _roleRepo.Save(data);
                        if (result.success)
                        {
                            //AuditLog(nameof(RoleController), nameof(Role), Constants.UpdateAction);
                        }
                    }
                    else
                    {
                        result.messages.Add(Constants.DuplicateMessage);
                    }
                }
                else
                {
                    Role? data = new Role();
                    if (!isDuplicate(data, vm))
                    {
                        data = mapper.MapViewModelToModel(data, vm);
                        data.created_by = GetLoggedInUserId();
                        result = _roleRepo.Save(data);
                        if (result.success)
                        {
                            //AuditLog(nameof(RoleController), nameof(Role), Constants.CreateAction);
                        }
                    }
                    else
                    {
                        result.messages.Add(Constants.DuplicateMessage);
                    }
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
        [HttpGet]
        [Route("getbyid")]
        public JsonResult GetById(int id)
        {
            RoleViewModel vm = new RoleViewModel();
            try
            {
                Role? data = _roleRepo.Get(id);
                vm = mapper.MapModelToViewModel(data, vm);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(vm);
        }
        [HttpDelete]
        [Route("delete")]
        public JsonResult Delete(int id)
        {
            CommandResult<Role> result = new CommandResult<Role>();
            try
            {
                Role? data = _roleRepo.Get(id);
                if (data != null)
                {
                    result = _roleRepo.Remove(data);
                    if (result.success)
                    {
                        //AuditLog(nameof(RoleController), nameof(Role), Constants.DeleteAction);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return Json(result);
        }
        protected bool isDuplicate(Role data, RoleViewModel vm)
        {
            bool duplicate = false;
            if (data.id > 0)
            {
                if (vm.name == data.name)
                {
                    duplicate = false;
                }
                else
                {
                    Role? u = _roleRepo.FindByName(vm.name);
                    if (u != null)
                    {
                        duplicate = true;
                    }
                }
            }
            else
            {
                Role? u = _roleRepo.FindByName(vm.name);
                if (u != null)
                {
                    duplicate = true;
                }
            }
            return duplicate;
        }
    }
}
