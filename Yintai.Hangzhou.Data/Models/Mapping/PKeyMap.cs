using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class PKeyEntityMap : EntityTypeConfiguration<PKeyEntity>
    {
        public PKeyEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PKey1)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Channel)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("PKey");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PKey1).HasColumnName("PKey");
            this.Property(t => t.Channel).HasColumnName("Channel");
            this.Property(t => t.Status).HasColumnName("Status");
		Init();
        }

		partial void Init();
    }
}
