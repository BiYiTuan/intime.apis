using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class CategoryPropertyValueEntityMap : EntityTypeConfiguration<CategoryPropertyValueEntity>
    {
        public CategoryPropertyValueEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ValueDesc)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("CategoryPropertyValue");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ValueDesc).HasColumnName("ValueDesc");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.PropertyId).HasColumnName("PropertyId");
		Init();
        }

		partial void Init();
    }
}
