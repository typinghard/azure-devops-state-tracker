using AzureDevopsTracker.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AzureDevopsTracker.Data.Mapping
{
    public class CustomFieldMapping : IEntityTypeConfiguration<WorkItemCustomField>
    {
        public void Configure(EntityTypeBuilder<WorkItemCustomField> builder)
        {
            builder.HasKey(k => new { k.WorkItemId, k.Key });

            builder.Property(p => p.Key)
                   .HasColumnType("varchar(1000)");

            builder.Property(p => p.Value)
                   .HasColumnType("varchar(max)");
        }
    }
}