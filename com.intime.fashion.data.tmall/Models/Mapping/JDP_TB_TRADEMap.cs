using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.intime.fashion.data.tmall.Models.Mapping
{
    public class JDP_TB_TRADEMap : EntityTypeConfiguration<JDP_TB_TRADE>
    {
        public JDP_TB_TRADEMap()
        {
            // Primary Key
            this.HasKey(t => new { t.tid, t.status, t.type, t.seller_nick, t.buyer_nick, t.created, t.modified, t.jdp_created, t.jdp_modified, t.jdp_hashcode, t.jdp_response });

            // Properties
            this.Property(t => t.tid)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.status)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.type)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.seller_nick)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.buyer_nick)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.jdp_hashcode)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.jdp_response)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("JDP_TB_TRADE");
            this.Property(t => t.tid).HasColumnName("tid");
            this.Property(t => t.status).HasColumnName("status");
            this.Property(t => t.type).HasColumnName("type");
            this.Property(t => t.seller_nick).HasColumnName("seller_nick");
            this.Property(t => t.buyer_nick).HasColumnName("buyer_nick");
            this.Property(t => t.created).HasColumnName("created");
            this.Property(t => t.modified).HasColumnName("modified");
            this.Property(t => t.jdp_created).HasColumnName("jdp_created");
            this.Property(t => t.jdp_modified).HasColumnName("jdp_modified");
            this.Property(t => t.jdp_hashcode).HasColumnName("jdp_hashcode");
            this.Property(t => t.jdp_response).HasColumnName("jdp_response");
        }
    }
}
