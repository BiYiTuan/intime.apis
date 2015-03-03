using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class tb_SeedEntityMap : EntityTypeConfiguration<tb_SeedEntity>
    {
        public tb_SeedEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.id);

            // Properties
            this.Property(t => t.id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("tb_Seed");
            this.Property(t => t.id).HasColumnName("id");
            this.Property(t => t.value).HasColumnName("value");
        }
    }
}
