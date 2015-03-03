using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class OPC_StockPropertyEntityMap : EntityTypeConfiguration<OPC_StockPropertyEntity>
    {
        public OPC_StockPropertyEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Channel)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.PropertyDesc)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("OPC_StockProperty");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Channel).HasColumnName("Channel");
            this.Property(t => t.PropertyDesc).HasColumnName("PropertyDesc");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ChannelPropertyId).HasColumnName("ChannelPropertyId");
		Init();
        }

		partial void Init();
    }
}
