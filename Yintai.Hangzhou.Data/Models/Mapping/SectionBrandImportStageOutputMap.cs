using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class SectionBrandImportStageOutputEntityMap : EntityTypeConfiguration<SectionBrandImportStageOutputEntity>
    {
        public SectionBrandImportStageOutputEntityMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Code, t.Name });

            // Properties
            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SectionBrandImportStageOutput");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
		Init();
        }

		partial void Init();
    }
}
