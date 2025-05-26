using DIS.DataAccess.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DIS.DataAccess.Entity_Configuration
{
    public class LogEntityConfiguration : IEntityTyperConfiguration<Log>
    {
        public void Configure(EntityTypeBuilder<Log> builder)
        {
        }
    }
}
