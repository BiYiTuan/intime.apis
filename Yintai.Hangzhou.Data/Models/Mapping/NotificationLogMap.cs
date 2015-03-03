using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class NotificationLogEntityMap : EntityTypeConfiguration<NotificationLogEntity>
    {
        public NotificationLogEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("NotificationLog");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.NotifyDate).HasColumnName("NotifyDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.SourceType).HasColumnName("SourceType");
            this.Property(t => t.SourceId).HasColumnName("SourceId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
		Init();
        }

		partial void Init();
    }
}
