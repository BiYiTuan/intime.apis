using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class testtableEntityMap : EntityTypeConfiguration<testtableEntity>
    {
        public testtableEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.number);

            // Properties
            this.Property(t => t.name)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("testtable");
            this.Property(t => t.number).HasColumnName("number");
            this.Property(t => t.name).HasColumnName("name");
            this.Property(t => t.content).HasColumnName("content");
            this.Property(t => t.time).HasColumnName("time");
            this.Property(t => t.money).HasColumnName("money");
            this.Property(t => t.age).HasColumnName("age");
            this.Property(t => t.height).HasColumnName("height");
            this.Property(t => t.more).HasColumnName("more");
		Init();
        }

		partial void Init();
    }
}
