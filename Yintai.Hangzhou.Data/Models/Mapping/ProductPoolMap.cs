using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class ProductPoolEntityMap : EntityTypeConfiguration<ProductPoolEntity>
    {
        public ProductPoolEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.MergedProductCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ErrorMessage)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("ProductPool");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.ChannelId).HasColumnName("ChannelId");
            this.Property(t => t.POIUploaded).HasColumnName("POIUploaded");
            this.Property(t => t.MergedProductCode).HasColumnName("MergedProductCode");
            this.Property(t => t.StoreId).HasColumnName("StoreId");
            this.Property(t => t.IsDefault).HasColumnName("IsDefault");
            this.Property(t => t.ErrorMessage).HasColumnName("ErrorMessage");
		Init();
        }

		partial void Init();
    }
}
