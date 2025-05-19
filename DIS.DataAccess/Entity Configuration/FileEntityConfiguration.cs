using DIS.DataAccess.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity_Configuration
{
    public class File_TBEntityConfiguration: IEntityTypeConfiguration<File_TB>

    {
        public void Configure(EntityTypeBuilder<File_TB> builder)
        {

        }
    }
}
