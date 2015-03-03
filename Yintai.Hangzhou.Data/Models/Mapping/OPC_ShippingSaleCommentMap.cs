using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class OPC_ShippingSaleCommentEntityMap : EntityTypeConfiguration<OPC_ShippingSaleCommentEntity>
    {
        public OPC_ShippingSaleCommentEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ShippingCode)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Content)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("OPC_ShippingSaleComment");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ShippingCode).HasColumnName("ShippingCode");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
            this.Property(t => t.ShippingSaleId).HasColumnName("ShippingSaleId");
		Init();
        }

		partial void Init();
    }
}
