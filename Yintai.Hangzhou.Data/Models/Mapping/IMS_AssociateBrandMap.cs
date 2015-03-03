using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class IMS_AssociateBrandEntityMap : EntityTypeConfiguration<IMS_AssociateBrandEntity>
    {
        public IMS_AssociateBrandEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("IMS_AssociateBrand");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.AssociateId).HasColumnName("AssociateId");
            this.Property(t => t.BrandId).HasColumnName("BrandId");
		Init();
        }

		partial void Init();
    }
}
