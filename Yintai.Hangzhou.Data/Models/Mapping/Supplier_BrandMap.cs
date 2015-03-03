using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class Supplier_BrandEntityMap : EntityTypeConfiguration<Supplier_BrandEntity>
    {
        public Supplier_BrandEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Supplier_Brand");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Supplier_Id).HasColumnName("Supplier_Id");
            this.Property(t => t.Brand_Id).HasColumnName("Brand_Id");
		Init();
        }

		partial void Init();
    }
}
