using eHandbook.modules.ManualManagement.CoreDomain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace eHandbook.modules.ManualManagement.Infrastructure.Configuration
{
    /// <summary>
    /// better way for creating a Fluent API configuration by using the IEntityTypeConfiguration interface. 
    /// By using it, we can divide the configuration for each entity into its own separate configuration class.
    /// In our OnModerCreating of DbContext class, we has to apply this configuration.
    /// </summary>
    internal class ManualConfiguration : IEntityTypeConfiguration<ManualEntity>
    {
        /// <summary>
        /// Entity Framework Core maps all the properties into the table columns.
        /// </summary>
        /// <param name="EntityTypeBuilder of type ManualEntity"></param>
        public void Configure(EntityTypeBuilder<ManualEntity> ModelBuilder)
        {
            ModelBuilder.ToTable("Manuals");
            ModelBuilder.HasKey(e => e.Id)
                .HasName("PrimaryKey_ManualId");
            ModelBuilder.HasIndex(e => new { e.Id })
                .IsUnique();
            ModelBuilder.Property(e => e.Id).ValueGeneratedOnAdd().HasValueGenerator<GuidValueGenerator>();
            ModelBuilder.Property(e => e.Description)
                .HasColumnName("Description")
                .HasMaxLength(50);
            ModelBuilder.Property(e => e.Path)
                .HasColumnName("Path");

            ///Fluent API for Seeding data base.
            ModelBuilder.HasData
                (
                new ManualEntity
                {
                    Id = Guid.NewGuid(),
                    Description = "Description",
                    Path = "http//wwww.path1.example"
                },
                 new ManualEntity
                 {
                     Id = Guid.NewGuid(),
                     Description = "Description2",
                     Path = "http//wwww.path2.example"
                 }
                );
        }
    }
}
