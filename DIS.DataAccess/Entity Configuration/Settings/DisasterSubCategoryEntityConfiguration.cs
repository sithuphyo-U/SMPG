using DIS.DataAccess.Entity.Settings;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity_Configuration.Settings
{
    public class DisasterSubCategoryEntityConfiguration : IEntityTypeConfiguration<DisasterSubCategory>
    {
        public void Configure(EntityTypeBuilder<DisasterSubCategory> builder)
        {

        }
    }
}
