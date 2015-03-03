using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class OPC_AuthRoleMenuEntityMap : EntityTypeConfiguration<OPC_AuthRoleMenuEntity>
    {
        public OPC_AuthRoleMenuEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("OPC_AuthRoleMenu");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OPC_AuthMenuId).HasColumnName("OPC_AuthMenuId");
            this.Property(t => t.OPC_AuthRoleId).HasColumnName("OPC_AuthRoleId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateUserId).HasColumnName("CreateUserId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdateUserId).HasColumnName("UpdateUserId");
		Init();
        }

		partial void Init();
    }
}
