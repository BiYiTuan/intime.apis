using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class IMS_SectionBrandEntityMap : EntityTypeConfiguration<IMS_SectionBrandEntity>
    {
        public IMS_SectionBrandEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("IMS_SectionBrand");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.SectionId).HasColumnName("SectionId");
            this.Property(t => t.BrandId).HasColumnName("BrandId");
		Init();
        }

		partial void Init();
    }
}
