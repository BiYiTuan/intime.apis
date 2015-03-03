using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class InventoryEntityMap : EntityTypeConfiguration<InventoryEntity>
    {
        public InventoryEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Inventory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.PColorId).HasColumnName("PColorId");
            this.Property(t => t.PSizeId).HasColumnName("PSizeId");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
            this.Property(t => t.ChannelInventoryId).HasColumnName("ChannelInventoryId");
		Init();
        }

		partial void Init();
    }
}
