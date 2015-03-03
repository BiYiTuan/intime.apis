using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class Promotion2ProductEntityMap : EntityTypeConfiguration<Promotion2ProductEntity>
    {
        public Promotion2ProductEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Promotion2Product");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProdId).HasColumnName("ProdId");
            this.Property(t => t.ProId).HasColumnName("ProId");
            this.Property(t => t.Status).HasColumnName("Status");
		Init();
        }

		partial void Init();
    }
}
