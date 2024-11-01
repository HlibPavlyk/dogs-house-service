using DogsHouseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DogsHouseService.Infrastructure.EntityTypeConfigurations;

public class DogConfiguration : IEntityTypeConfiguration<Dog>
{
    public void Configure(EntityTypeBuilder<Dog> builder)
    {
        builder.ToTable("dogs");
        
        builder.HasKey(d => d.Name);

        builder.HasIndex(d => d.Name).IsUnique();
        
        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("name");

        builder.Property(d => d.Color)
            .IsRequired()
            .HasMaxLength(30)
            .HasColumnName("color");

        builder.Property(d => d.TailLength)
            .IsRequired()
            .HasColumnName("tail_length");

        builder.Property(d => d.Weight)
            .IsRequired()
            .HasColumnName("weight");
    }
}