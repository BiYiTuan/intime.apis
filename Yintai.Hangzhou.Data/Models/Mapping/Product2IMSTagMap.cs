using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class Product2IMSTagEntityMap : EntityTypeConfiguration<Product2IMSTagEntity>
    {
        public Product2IMSTagEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Product2IMSTag");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.IMSTagId).HasColumnName("IMSTagId");
		Init();
        }

		partial void Init();
    }
}
