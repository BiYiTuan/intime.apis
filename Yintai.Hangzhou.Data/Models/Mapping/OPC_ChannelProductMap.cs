using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class OPC_ChannelProductEntityMap : EntityTypeConfiguration<OPC_ChannelProductEntity>
    {
        public OPC_ChannelProductEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("OPC_ChannelProduct");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.ColorId).HasColumnName("ColorId");
            this.Property(t => t.ChannelId).HasColumnName("ChannelId");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.LabelPrice).HasColumnName("LabelPrice");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.UpDateTime).HasColumnName("UpDateTime");
            this.Property(t => t.DownDateTime).HasColumnName("DownDateTime");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedUser).HasColumnName("CreatedUser");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
            this.Property(t => t.UpdatedUser).HasColumnName("UpdatedUser");
		Init();
        }

		partial void Init();
    }
}
