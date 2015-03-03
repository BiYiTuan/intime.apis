using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class SeedEntityMap : EntityTypeConfiguration<SeedEntity>
    {
        public SeedEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Name);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("Seed");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Value).HasColumnName("Value");
		Init();
        }

		partial void Init();
    }
}
