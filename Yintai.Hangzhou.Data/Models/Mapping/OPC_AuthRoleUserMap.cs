using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class OPC_AuthRoleUserEntityMap : EntityTypeConfiguration<OPC_AuthRoleUserEntity>
    {
        public OPC_AuthRoleUserEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("OPC_AuthRoleUser");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OPC_AuthUserId).HasColumnName("OPC_AuthUserId");
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
