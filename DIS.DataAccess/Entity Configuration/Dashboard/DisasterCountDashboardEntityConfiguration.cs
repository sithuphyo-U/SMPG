using DIS.DataAccess.Entity.Dashboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity_Configuration.Dashboard
{
    public class DisasterCountDashboardEntityConfiguration : IEntityTypeConfiguration<DisasterCountDashboard>
    {
        public void Configure(EntityTypeBuilder<DisasterCountDashboard> builder)
        {
            builder.HasNoKey();
            builder.Property(e => e.DisasterCategoryId)
                  .HasColumnName("disasterCategory_id");

            builder.Property(e => e.CategoryName)
                   .HasColumnName("CategoryName");

            builder.Property(e => e.Year)
                   .HasColumnName("Year");

            builder.Property(e => e.Month)
                   .HasColumnName("Month");

            builder.Property(e => e.TotalRecords)
                   .HasColumnName("TotalRecords");
        }
    }
}
