using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class IMS_GiftCardItemEntityMap : EntityTypeConfiguration<IMS_GiftCardItemEntity>
    {
        public IMS_GiftCardItemEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("IMS_GiftCardItem");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.GiftCardId).HasColumnName("GiftCardId");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.UnitPrice).HasColumnName("UnitPrice");
            this.Property(t => t.MaxLimit).HasColumnName("MaxLimit");
            this.Property(t => t.Status).HasColumnName("Status");
		Init();
        }

		partial void Init();
    }
}
