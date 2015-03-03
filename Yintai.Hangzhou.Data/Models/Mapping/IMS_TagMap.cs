using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class IMS_TagEntityMap : EntityTypeConfiguration<IMS_TagEntity>
    {
        public IMS_TagEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("IMS_Tag");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdateUser).HasColumnName("UpdateUser");
            this.Property(t => t.SortOrder).HasColumnName("SortOrder");
            this.Property(t => t.Visible4Display).HasColumnName("Visible4Display");
            this.Property(t => t.ImmediatePublic).HasColumnName("ImmediatePublic");
            this.Property(t => t.Only4Tmall).HasColumnName("Only4Tmall");
		Init();
        }

		partial void Init();
    }
}
