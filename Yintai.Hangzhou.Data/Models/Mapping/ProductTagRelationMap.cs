using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class ProductTagRelationEntityMap : EntityTypeConfiguration<ProductTagRelationEntity>
    {
        public ProductTagRelationEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("ProductTagRelations");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Product_Id).HasColumnName("Product_Id");
            this.Property(t => t.Tag_Id).HasColumnName("Tag_Id");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedUser).HasColumnName("CreatedUser");
        }
    }
}
