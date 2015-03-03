using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class Group_AliKeysEntityMap : EntityTypeConfiguration<Group_AliKeysEntity>
    {
        public Group_AliKeysEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ParterId)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Md5Key)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.SellerAccount)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("Group_AliKeys");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.ParterId).HasColumnName("ParterId");
            this.Property(t => t.Md5Key).HasColumnName("Md5Key");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.SellerAccount).HasColumnName("SellerAccount");
		Init();
        }

		partial void Init();
    }
}
