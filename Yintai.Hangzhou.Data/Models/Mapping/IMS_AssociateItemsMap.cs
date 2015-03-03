using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class IMS_AssociateItemsEntityMap : EntityTypeConfiguration<IMS_AssociateItemsEntity>
    {
        public IMS_AssociateItemsEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("IMS_AssociateItems");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.AssociateId).HasColumnName("AssociateId");
            this.Property(t => t.ItemType).HasColumnName("ItemType");
            this.Property(t => t.ItemId).HasColumnName("ItemId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
		Init();
        }

		partial void Init();
    }
}
