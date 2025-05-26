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
    public class RecentDisasterLogDashboardEntityConfiguration : IEntityTypeConfiguration<RecentDisasterLogDashboard>
    {
        public void Configure(EntityTypeBuilder<RecentDisasterLogDashboard> builder)
        {
            builder.HasNoKey();
            builder.ToView(null);
        }
    }
}
