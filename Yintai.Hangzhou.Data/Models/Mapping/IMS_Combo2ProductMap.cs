using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class IMS_Combo2ProductEntityMap : EntityTypeConfiguration<IMS_Combo2ProductEntity>
    {
        public IMS_Combo2ProductEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("IMS_Combo2Product");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ComboId).HasColumnName("ComboId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
		Init();
        }

		partial void Init();
    }
}
