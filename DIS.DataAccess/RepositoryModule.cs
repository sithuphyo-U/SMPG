using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess
{
    public static class RepositoryModule
    {
        public static Type? GetRepositories(Type type)
        {
            Assembly ass = Assembly.Load("DIS.DataAccess");
            // Get all types from the assembly
            Type[] types = ass.GetTypes();
            Type? t = null;
            // Filter types to get only interfaces
            var repos = types.Where(t => type.IsAssignableFrom(t) && !t.IsInterface);
            //Repository Configuration
            foreach (var repo in repos)
            {
                if (repo != null)
                {
                    t = repo;
                }

            }
            return t;
        }
    }
}
