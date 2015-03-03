using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class RMAReasonEntityMap : EntityTypeConfiguration<RMAReasonEntity>
    {
        public RMAReasonEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Reason)
                .IsRequired()
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("RMAReason");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Reason).HasColumnName("Reason");
            this.Property(t => t.Status).HasColumnName("Status");
		Init();
        }

		partial void Init();
    }
}
