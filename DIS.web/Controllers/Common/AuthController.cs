using DIS.Application.Service;
using DIS.DataAccess.Entity;
using DIS.DataAccess.Interfaces;
using DIS.Infrastructure.Utilities;
using DIS.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DIS.Web.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : BaseController
    {
        IUserRepository _userRepo;

        public AuthController(IUserRepository userRepo) : base(typeof(AuthController))
        {
            _userRepo = userRepo;
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        public JsonResult Login(LoginViewModel vm)
        {
            UserEntryViewModel loginUser = new UserEntryViewModel();
            try
            {
                User? user = _userRepo.FindByUserName(vm.username);
                if (user != null)
                {
                    if (user.status)
                    {
                        if (PasswordService.VerifyPassword(vm.password, user.password))
                        {

                            var token = CreateJWT(user, ConfigManager.GetSecretKey());
                            loginUser.token = token;
                            loginUser.success = true;
                            loginUser.id = user.id;
                            loginUser.username = user.username;
                            loginUser.role = user.role;
                            loginUser.name = user.name;
                        }
                        else
                        {
                            loginUser.success = false;
                            loginUser.messages.Add(Constants.IncorrectPasswordMessage);
                        }
                    }
                    else
                    {
                        user.status = false;
                    }
                    
                }
                else
                {
                    loginUser.success = false;
                    loginUser.messages.Add(Constants.IncorrectUserNameMessage);
                }
            }
            catch (Exception ex)
            {
                loginUser.messages.Clear();
                loginUser.messages.Add(ex.Message);
                logger.LogError(ex.Message);
            }
            return Json(loginUser);
        }
    }
}
