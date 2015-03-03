using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class OPC_RMALogEntityMap : EntityTypeConfiguration<OPC_RMALogEntity>
    {
        public OPC_RMALogEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Operation)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("OPC_RMALog");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OpcRmaId).HasColumnName("OpcRmaId");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.Operation).HasColumnName("Operation");
		Init();
        }

		partial void Init();
    }
}
