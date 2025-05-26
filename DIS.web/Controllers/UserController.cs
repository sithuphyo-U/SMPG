using DIS.Application.Service;
using DIS.DataAccess.Entity;
using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Utilities;
using DIS.Web.Controllers.Common;
using DIS.Web.Mappers;
using DIS.Web.Mappers.Setttings;
using DIS.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NPOI.SS.Formula.Functions;
using System.Linq.Expressions;

namespace DIS.Web.Controllers.Settings
{
    [Route("api/[controller]")]
    [ApiController]

    public class UserController : BaseController
    { 
    IUserRepository _userRepo;
    UserMapper mapper;
    public UserController(IUserRepository userRepo)
        : base(typeof(UserController))
    {
        _userRepo = userRepo;
        mapper = new UserMapper();
    }
    [HttpGet]
    public JsonResult Get()
    {
        PagedResult<UserViewModel> list = new PagedResult<UserViewModel>();
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
    private PagedResult<UserViewModel> GetAllData()
    {
        QueryOptions<User> queryoption = GetQueryOptions<User>();
        UserViewModel vm = GetRequestParameters();
        queryoption = mapper.PrepareQueryOptionForRepository(queryoption, vm);
        PagedResult<User> list = _userRepo.GetPagedResults(queryoption);
        PagedResult<UserViewModel> vmList = mapper.MapModelToListViewMode(list);
        return vmList;
    }
    private UserViewModel GetRequestParameters()
    {
        UserViewModel vm = new UserViewModel();
        vm.name = Request.Query["name"].ToString();
        vm.username = Request.Query["username"].ToString();
        string temp_role_id = Request.Query["role"].ToString();
        string tempStatus = Request.Query["status"].ToString();
        if (!string.IsNullOrEmpty(tempStatus))
        {
            vm.status = Convert.ToBoolean(tempStatus);
        }
        if (!string.IsNullOrEmpty(temp_role_id))
        {
            vm.role_id = Convert.ToInt32(temp_role_id);
        }
        return vm;
    }
    [HttpPost]
    [Route("SaveOrUpdate")]
    public IActionResult SaveOrUpdate(UserViewModel vm)
    {
        CommandResult<User> result = new CommandResult<User>();
        try
        {
            if (vm.id > 0)
            {
                User? user = _userRepo.Get(vm.id);
                if (!isDuplicate(user, vm))
                {
                    user = mapper.MapViewModelToModel(user, vm);
                    result = _userRepo.Save(user);
                    if (result.success)
                    {
                        AuditLog(nameof(UserController), nameof(User), Constants.UpdateAction);
                    }
                }
                else
                {
                    result.messages.Add(Constants.DuplicateUserName);
                }
            }
            else
            {
                User? user = new User();
                if (!isDuplicate(user, vm))
                {
                    user = mapper.MapViewModelToModel(user, vm);
                    user.password = PasswordService.HashPassword(vm.password);
                    result = _userRepo.Save(user);
                    if (result.success)
                    {
                        AuditLog(nameof(UserController), nameof(User), Constants.CreateAction);
                    }
                }
                else
                {
                    result.messages.Add(Constants.DuplicateUserName);
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

        [HttpPost]
        [Route("ManageAccount")]
        public IActionResult GetManageAccount(UserViewModel vm)
        {
            CommandResult<User> result = new CommandResult<User>();
            try
            {
                if (vm.id > 0)
                {
                    User? user = _userRepo.Get(vm.id);
                    if (!isDuplicate(user, vm))
                    {
                        user = mapper.MapViewModelToModel(user, vm);
                        result = _userRepo.Save(user);
                        if (result.success)
                        {
                            AuditLog(nameof(UserController), nameof(User), Constants.UpdateAction);
                        }
                    }
                    else
                    {
                        result.messages.Add(Constants.DuplicateUserName);
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
        UserViewModel vm = new UserViewModel();
        try
        {
            User? user = _userRepo.Get(id);
            vm = mapper.MapModelToViewModel(user, vm);
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
        CommandResult<User> result = new CommandResult<User>();
        try
        {
            User? user = _userRepo.Get(id);
            if (user != null)
            {
                result = _userRepo.Remove(user);
                if (result.success)
                {
                    AuditLog(nameof(UserController), nameof(User), Constants.DeleteAction);
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
    [Route("reset_password")]

    public JsonResult ResetPassword(UserViewModel vm)
    {
        CommandResult<User> result = new CommandResult<User>();
        try
        {
            User user = _userRepo.Get(vm.id);

            if (user != null)
            {
                user.password = PasswordService.HashPassword(vm.password);
                _userRepo.Save(user);

                result.success = true;
                result.messages.Add("Password changed successfully");
            }


        }
        catch (Exception ex)
        {
            result?.messages.Add(ex.Message);
            logger.LogError(ex.Message);
        }
        return Json(result);
    }

        protected bool isDuplicate(User user, UserViewModel vm)
    {
        bool duplicate = false;
        if (user.id > 0)
        {
            if (vm.username == user.username)
            {
                duplicate = false;
            }
            else
            {
                User? u = _userRepo.FindByUserName(vm.username);
                if (u != null)
                {
                    duplicate = true;
                }
            }
        }
        else
        {
            User? u = _userRepo.FindByUserName(vm.username);
            if (u != null)
            {
                duplicate = true;
            }
        }
        return duplicate;
    }

}
}
