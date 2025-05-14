using DIS.DataAccess.Entity.Settings;
using DIS.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity_Configuration.Settings
{
    public class CountryTypeEntityConfiguration : IEntityTypeConfiguration<CountryType>
    {
        public void Configure(EntityTypeBuilder<CountryType> builder)
        {
            
        }
    }
}
