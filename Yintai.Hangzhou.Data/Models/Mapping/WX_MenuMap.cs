
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class WX_MenuEntityMap : EntityTypeConfiguration<WX_MenuEntity>
    {
        public WX_MenuEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            

            // Table & Column Mappings
            this.ToTable("WX_Menu");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ActionType).HasColumnName("ActionType");
            this.Property(t => t.AppId).HasColumnName("AppId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.Url).HasColumnName("Url");
            this.Property(t => t.Pos).HasColumnName("Pos");
            this.Property(t => t.WKey).HasColumnName("WKey");
          
		Init();
        }

		partial void Init();
    }
}
