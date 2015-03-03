using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class Ref_EnumerationMemberEntityMap : EntityTypeConfiguration<Ref_EnumerationMemberEntity>
    {
        public Ref_EnumerationMemberEntityMap()
        {
            // Primary Key
            this.HasKey(t => new { t.EnumerationName, t.Name });

            // Properties
            this.Property(t => t.EnumerationName)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("Ref_EnumerationMember");
            this.Property(t => t.EnumerationName).HasColumnName("EnumerationName");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
