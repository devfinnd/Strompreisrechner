using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinnD.Strompreisrechner.Api.Database;

public sealed class HistoryEntryConfiguration : IEntityTypeConfiguration<HistoryEntry>
{
    public void Configure(EntityTypeBuilder<HistoryEntry> builder)
    {
        builder.ToTable("history_entries");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("id").IsRequired();
        builder.Property(x => x.KwhConsumptionPerYear).HasColumnName("kwh_per_year").IsRequired();
        builder.Property(x => x.PricePerKwh).HasColumnName("price_per_kwh").IsRequired();
        builder.Property(x => x.PricePerYear).HasColumnName("price_per_year").IsRequired();
        builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired()
            .HasDefaultValueSql("datetime('now')");
    }
}
