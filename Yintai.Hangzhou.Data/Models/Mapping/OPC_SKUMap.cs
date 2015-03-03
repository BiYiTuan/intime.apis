using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class OPC_SKUEntityMap : EntityTypeConfiguration<OPC_SKUEntity>
    {
        public OPC_SKUEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("OPC_SKU");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.ColorValueId).HasColumnName("ColorValueId");
            this.Property(t => t.SizeValueId).HasColumnName("SizeValueId");
		Init();
        }

		partial void Init();
    }
}
