using Azure.Core;
using DIS.DataAccess.Entity;
using DIS.DataAccess;
using DIS.Infrastructure.Common;
using DIS.Infrastructure.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DIS.Infrastructure.Enumerations;
using DIS.Infrastructure.Logging;
using DIS.DataAccess.Interfaces;
using DIS.Infrastruture.Enumerations;

namespace DIS.Web.Controllers.Common
{
    public abstract class BaseController : Microsoft.AspNetCore.Mvc.Controller

    {

        protected Logger logger;
        public BaseController(Type type)
        {
            logger = new Logger(type);
        }
        protected TType GetRequestParameter<TType>(string key)
            {
                TType val = default(TType);
                try
                {

                    if (Request.Query[key].ToString() != null)
                    {
                        string tmp = Request.Query[key].FirstOrDefault();
                        val = (TType)Convert.ChangeType(tmp, typeof(TType));
                    }
                }
                catch (Exception ex)
                {
                    val = default(TType);
                }

                return val;
            }

            protected QueryOptions<TEntity> GetQueryOptions<TEntity>() where TEntity : BaseEntity
            {
                QueryOptions<TEntity> queryOptions = new QueryOptions<TEntity>();
                string length = Request.Query["itemsPerPage"].FirstOrDefault();
                if (!string.IsNullOrEmpty(length))
                {
                    queryOptions.legth = Convert.ToInt32(length);
                    queryOptions.RecordPerPage = Convert.ToInt32(length);
                }
                string page = Request.Query["page"].ToString();
                if (!string.IsNullOrEmpty(page))
                {
                    queryOptions.Page = Convert.ToInt32(page);
                }
               
                var SortColumn = Request.Query["sortBy"].FirstOrDefault();
                if (!string.IsNullOrEmpty(SortColumn))
                {
                    queryOptions.SortColumnName = SortColumn;
                }
                var SortColumnDirection = Request.Query["sortOrder"].FirstOrDefault();
                if (!string.IsNullOrEmpty(SortColumnDirection))
                {
                    if (SortColumnDirection == "desc" || SortColumnDirection == "DESC")
                    {
                        queryOptions.SortOrder = SortOrder.DESC;
                    }
                    else
                    {
                        queryOptions.SortOrder = SortOrder.ASC;
                    }
                }
                else
                {
                    queryOptions.SortOrder = SortOrder.DESC;
                }
                queryOptions.SearchValue = Request.Query["search"].FirstOrDefault();
                return queryOptions;
            }
            protected int GetLoggedInUserId()
            {
                int id = 0;
                if (User.Identity.IsAuthenticated)
                {
                    id = Convert.ToInt32(User.Identity.Name);
                }
                return id;
            }
        [NonAction]
        public void AuditLog(string controller, string table, string action)
        {
            try
            {
                Task task = Task.Run(() =>
                {
                    try
                    {
                        var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                        Log log = new Log();
                        log.user_id = GetLoggedInUserId();
                        log.controller = controller;
                        log.table = table;
                        log.action = action;
                        log.date = DateTime.Now;
                        log.ip = remoteIpAddress.ToString();
                        log.url = Request.Path + Request.QueryString;
                        using (var context = new AuditDbContext())
                        {
                            context.Set<Log>().Add(log);
                            context.Entry(log).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
        [NonAction]
        public void LoginAuditLog(string controller, string table, string action, int id)
        {
            try
            {
                Task task = Task.Run(() =>
                {
                    try
                    {
                        var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
                        Log log = new Log();
                        log.user_id = id;
                        log.controller = controller;
                        log.table = table;
                        log.action = action;
                        log.date = DateTime.Now;
                        log.ip = remoteIpAddress.ToString();
                        log.url = Request.Path + Request.QueryString;
                        using (var context = new AuditDbContext())
                        {
                            context.Set<Log>().Add(log);
                            context.Entry(log).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                            context.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message);
                    }

                });
                task.Wait();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
        }
        protected bool hasPermission(IRoleRepository roleRepo, AuditAction action)
        {
            bool hasPermission = false;
            if (User.Identity.IsAuthenticated)
            {
                var roleClaim = User.FindFirst(ClaimTypes.Role);
                if (roleClaim != null)
                {
                    string? id = roleClaim.Value;
                    Role? role = roleRepo.Get(Convert.ToInt32(id));

                }

            }
            return hasPermission;
        }

        protected string CreateJWT(User user, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);
            string id = user.id.ToString();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {

                new Claim(ClaimTypes.Name, id),
                    // Add additional claims as needed
                }),
                Expires = DateTime.UtcNow.AddDays(1), // Token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
    }

