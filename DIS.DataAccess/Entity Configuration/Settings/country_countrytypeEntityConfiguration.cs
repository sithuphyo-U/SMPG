using DIS.DataAccess.Entity.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity_Configuration.Settings
{
    public class country_countrytypeEntityConfiguration : IEntityTypeConfiguration<country_countrytype>

    {
        public void Configure(EntityTypeBuilder<country_countrytype> builder)
        {
           
        }
    }
}
