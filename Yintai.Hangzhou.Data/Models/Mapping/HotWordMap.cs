using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class HotWordEntityMap : EntityTypeConfiguration<HotWordEntity>
    {
        public HotWordEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Word)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("HotWord");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Word).HasColumnName("Word");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreatedUser).HasColumnName("CreatedUser");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.UpdatedUser).HasColumnName("UpdatedUser");
            this.Property(t => t.UpdatedDate).HasColumnName("UpdatedDate");
		Init();
        }

		partial void Init();
    }
}
