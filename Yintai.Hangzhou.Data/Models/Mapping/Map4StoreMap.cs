using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class Map4StoreEntityMap : EntityTypeConfiguration<Map4StoreEntity>
    {
        public Map4StoreEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Province)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.City)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.ChannelStoreId)
                .HasMaxLength(20);

            this.Property(t => t.Channel)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Map4Store");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.StoreId).HasColumnName("StoreId");
            this.Property(t => t.Province).HasColumnName("Province");
            this.Property(t => t.City).HasColumnName("City");
            this.Property(t => t.ChannelStoreId).HasColumnName("ChannelStoreId");
            this.Property(t => t.Channel).HasColumnName("Channel");
            this.Property(t => t.ChannelId).HasColumnName("ChannelId");
		Init();
        }

		partial void Init();
    }
}
