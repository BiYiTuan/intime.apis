using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class PaymentNotifyLogEntityMap : EntityTypeConfiguration<PaymentNotifyLogEntity>
    {
        public PaymentNotifyLogEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PaymentCode)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.OrderNo)
                .HasMaxLength(100);

            this.Property(t => t.PaymentContent)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("PaymentNotifyLog");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PaymentCode).HasColumnName("PaymentCode");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.PaymentContent).HasColumnName("PaymentContent");
		Init();
        }

		partial void Init();
    }
}
