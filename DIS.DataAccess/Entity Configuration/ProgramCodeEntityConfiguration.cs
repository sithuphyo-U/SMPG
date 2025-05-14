using DIS.DataAccess.Entity;
using DIS.Infrastructure.Common;
using DIS.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity_Configuration
{
    public class ProgramCodeEntityConfiguration : IEntityTypeConfiguration<ProgramCode>
    {
        public void Configure(EntityTypeBuilder<ProgramCode> builder)
        {
           
        }
    }
}
