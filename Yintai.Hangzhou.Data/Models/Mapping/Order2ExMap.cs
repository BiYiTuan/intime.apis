using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class Order2ExEntityMap : EntityTypeConfiguration<Order2ExEntity>
    {
        public Order2ExEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.OrderNo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.ExOrderNo)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("Order2Ex");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.ExOrderNo).HasColumnName("ExOrderNo");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
		Init();
        }

		partial void Init();
    }
}
