using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.intime.fashion.data.tmall.Models.Mapping
{
    public class JDP_TB_ITEMMap : EntityTypeConfiguration<JDP_TB_ITEM>
    {
        public JDP_TB_ITEMMap()
        {
            // Primary Key
            this.HasKey(t => new { t.num_iid, t.nick, t.approve_status, t.cid, t.has_showcase, t.has_discount, t.created, t.modified, t.jdp_created, t.jdp_modified, t.jdp_delete, t.jdp_hashcode, t.jdp_response });

            // Properties
            this.Property(t => t.num_iid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.nick)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.approve_status)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.cid)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.has_showcase)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.has_discount)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.jdp_delete)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.jdp_hashcode)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.jdp_response)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("JDP_TB_ITEM");
            this.Property(t => t.num_iid).HasColumnName("num_iid");
            this.Property(t => t.nick).HasColumnName("nick");
            this.Property(t => t.approve_status).HasColumnName("approve_status");
            this.Property(t => t.cid).HasColumnName("cid");
            this.Property(t => t.has_showcase).HasColumnName("has_showcase");
            this.Property(t => t.has_discount).HasColumnName("has_discount");
            this.Property(t => t.created).HasColumnName("created");
            this.Property(t => t.modified).HasColumnName("modified");
            this.Property(t => t.jdp_created).HasColumnName("jdp_created");
            this.Property(t => t.jdp_modified).HasColumnName("jdp_modified");
            this.Property(t => t.jdp_delete).HasColumnName("jdp_delete");
            this.Property(t => t.jdp_hashcode).HasColumnName("jdp_hashcode");
            this.Property(t => t.jdp_response).HasColumnName("jdp_response");
        }
    }
}
