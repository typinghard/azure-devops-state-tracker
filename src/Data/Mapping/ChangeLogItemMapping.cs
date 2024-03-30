using AzureDevopsTracker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AzureDevopsTracker.Data.Mapping
{
    public class ChangeLogItemMapping : IEntityTypeConfiguration<ChangeLogItem>
    {
        public void Configure(EntityTypeBuilder<ChangeLogItem> builder)
        {
            builder.Property(d => d.Description)
                   .HasColumnType("varchar(max)");
        }
    }
}