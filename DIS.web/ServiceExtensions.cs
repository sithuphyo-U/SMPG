using DIS.DataAccess;

using System.Reflection;

namespace DIS.Web
{
    public static class ServiceExtensions
    {
        public static void AddRepositories(this IServiceCollection services, Assembly assembly)
        {
            services.AddScoped<IDbContext, DatabaseContext>();
            var interfaces = InterfaceModule.GetInterfaces();

            foreach (var iFace in interfaces)
            {
                var repo = RepositoryModule.GetRepositories(iFace);
                if (repo != null)
                {
                    services.AddScoped(iFace, repo);
                }
            }

        }
    }
}
