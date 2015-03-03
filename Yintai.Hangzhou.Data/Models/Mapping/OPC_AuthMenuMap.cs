using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class OPC_AuthMenuEntityMap : EntityTypeConfiguration<OPC_AuthMenuEntity>
    {
        public OPC_AuthMenuEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.MenuName)
                .HasMaxLength(40);

            this.Property(t => t.Url)
                .HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("OPC_AuthMenu");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MenuName).HasColumnName("MenuName");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateUserId).HasColumnName("CreateUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdateUserId).HasColumnName("UpdateUserId");
            this.Property(t => t.PraentMenuId).HasColumnName("PraentMenuId");
            this.Property(t => t.IsValid).HasColumnName("IsValid");
            this.Property(t => t.Sort).HasColumnName("Sort");
            this.Property(t => t.Url).HasColumnName("Url");
		Init();
        }

		partial void Init();
    }
}
