using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.intime.fashion.data.tmall.Models.Mapping
{
    public class OrderSyncMap : EntityTypeConfiguration<OrderSync>
    {
        public OrderSyncMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ImsOrderNo)
                .IsRequired()
                .HasMaxLength(30);

            // Table & Column Mappings
            this.ToTable("OrderSync");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.ImsOrderNo).HasColumnName("ImsOrderNo");
            this.Property(t => t.TmallOrderId).HasColumnName("TmallOrderId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.LogisticsSynced).HasColumnName("LogisticsSynced");
        }
    }
}
