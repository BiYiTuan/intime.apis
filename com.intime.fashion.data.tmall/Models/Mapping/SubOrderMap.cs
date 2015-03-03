using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.intime.fashion.data.tmall.Models.Mapping
{
    public class SubOrderMap : EntityTypeConfiguration<SubOrder>
    {
        public SubOrderMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Store)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("SubOrder");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TmallOrderId).HasColumnName("TmallOrderId");
            this.Property(t => t.TmallSubOrderId).HasColumnName("TmallSubOrderId");
            this.Property(t => t.LogisticsSynced).HasColumnName("LogisticsSynced");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.IsForceSynced).HasColumnName("IsForceSynced");
            this.Property(t => t.Store).HasColumnName("Store");
            this.Property(t => t.ImsInventoryId).HasColumnName("ImsInventoryId");
        }
    }
}
