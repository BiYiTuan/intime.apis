using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class OPC_StorePriorityEntityMap : EntityTypeConfiguration<OPC_StorePriorityEntity>
    {
        public OPC_StorePriorityEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("OPC_StorePriority");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.StoreId).HasColumnName("StoreId");
            this.Property(t => t.Priority).HasColumnName("Priority");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
		Init();
        }

		partial void Init();
    }
}
