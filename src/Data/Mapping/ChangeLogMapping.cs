using AzureDevopsTracker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AzureDevopsTracker.Data.Mapping
{
    public class ChangeLogMapping : IEntityTypeConfiguration<ChangeLog>
    {
        public void Configure(EntityTypeBuilder<ChangeLog> builder)
        {
            builder.Property(d => d.Response)
                   .HasColumnType("varchar(max)");
        }
    }
}