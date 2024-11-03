using CityDiIV.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CityDiIV.Persistence.Configs;

public class SampleDataConfig : IEntityTypeConfiguration<SampleData>
{
    public void Configure(EntityTypeBuilder<SampleData> builder)
    {
        builder.ToTable(nameof(SampleData), "Data");
        builder.Property(e => e.SampleString).HasMaxLength(1000).IsRequired();
    }
}
