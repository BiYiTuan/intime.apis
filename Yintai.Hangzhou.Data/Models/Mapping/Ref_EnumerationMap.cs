using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class Ref_EnumerationEntityMap : EntityTypeConfiguration<Ref_EnumerationEntity>
    {
        public Ref_EnumerationEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Name);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.BaseType)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.Description)
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("Ref_Enumeration");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.BaseType).HasColumnName("BaseType");
            this.Property(t => t.HasFlagsAttribute).HasColumnName("HasFlagsAttribute");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
