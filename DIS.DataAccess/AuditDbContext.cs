using DIS.Infrastructure.Utilities;
using DMS.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess
{
    public class AuditDbContext : DbContext
    {
        public AuditDbContext()
        {
        }
        public new DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseLazyLoadingProxies();
            optionsBuilder.UseSqlServer(ConfigManager.GetConnectionString()).UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);

        }
        // configure models
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //configure model from Assembly
            //using EntityConfiguratuion.IEntityTypeConfiguration<TEntity>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
