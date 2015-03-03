using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.intime.fashion.data.erp.Models.Mapping
{
    public class SALE_CODEMap : EntityTypeConfiguration<SALE_CODE>
    {
        public SALE_CODEMap()
        {
            // Primary Key
            this.HasKey(t => t.SID);

            // Properties
            this.Property(t => t.SID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SALE_CODE1)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.OPT_REAL_NAME)
                .HasMaxLength(40);

            this.Property(t => t.MEMO)
                .HasMaxLength(200);

            this.Property(t => t.SUPPLY_SHOP_CODE)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.COUNTER_TEL)
                .HasMaxLength(20);

            this.Property(t => t.HOURSE_NUMBER)
                .HasMaxLength(20);

            this.Property(t => t.IMPORT_GYS_SHOP_NO)
                .HasMaxLength(20);

            this.Property(t => t.SALE_CODE_NAME)
                .HasMaxLength(60);

            this.Property(t => t.ADDRESS)
                .HasMaxLength(200);

            this.Property(t => t.IMAGE_NAME)
                .HasMaxLength(100);

            this.Property(t => t.DESCRIPT)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("SALE_CODE", "YINTAIWEB");
            this.Property(t => t.SID).HasColumnName("SID");
            this.Property(t => t.SUPPLY_INFO_SID).HasColumnName("SUPPLY_INFO_SID");
            this.Property(t => t.SUPPLY_CLASS_SID).HasColumnName("SUPPLY_CLASS_SID");
            this.Property(t => t.SHOP_SID).HasColumnName("SHOP_SID");
            this.Property(t => t.SALE_CODE1).HasColumnName("SALE_CODE");
            this.Property(t => t.SUPPLY_SALING_BIT).HasColumnName("SUPPLY_SALING_BIT");
            this.Property(t => t.AREA_SID).HasColumnName("AREA_SID");
            this.Property(t => t.NET_SALE_BIT).HasColumnName("NET_SALE_BIT");
            this.Property(t => t.OPT_USER_SID).HasColumnName("OPT_USER_SID");
            this.Property(t => t.OPT_REAL_NAME).HasColumnName("OPT_REAL_NAME");
            this.Property(t => t.OPT_UPDATE_TIME).HasColumnName("OPT_UPDATE_TIME");
            this.Property(t => t.MEMO).HasColumnName("MEMO");
            this.Property(t => t.SUPPLY_SALING_TIME).HasColumnName("SUPPLY_SALING_TIME");
            this.Property(t => t.SUPPLY_SHOP_CODE).HasColumnName("SUPPLY_SHOP_CODE");
            this.Property(t => t.COUNTER_TEL).HasColumnName("COUNTER_TEL");
            this.Property(t => t.ACTIVE_BIT).HasColumnName("ACTIVE_BIT");
            this.Property(t => t.HOURSE_NUMBER).HasColumnName("HOURSE_NUMBER");
            this.Property(t => t.DISCOUNT_RATE_TYPE).HasColumnName("DISCOUNT_RATE_TYPE");
            this.Property(t => t.IMPORT_GYS_SHOP_NO).HasColumnName("IMPORT_GYS_SHOP_NO");
            this.Property(t => t.SALE_CODE_NAME).HasColumnName("SALE_CODE_NAME");
            this.Property(t => t.ADDRESS).HasColumnName("ADDRESS");
            this.Property(t => t.IMAGE_NAME).HasColumnName("IMAGE_NAME");
            this.Property(t => t.DISPLAY_NUM).HasColumnName("DISPLAY_NUM");
            this.Property(t => t.DESCRIPT).HasColumnName("DESCRIPT");
            this.Property(t => t.DISCOUNT_RATE_UNION).HasColumnName("DISCOUNT_RATE_UNION");
            this.Property(t => t.PRO_RULE_MEMBER_SID).HasColumnName("PRO_RULE_MEMBER_SID");
            this.Property(t => t.SC_OFF_UP).HasColumnName("SC_OFF_UP");
            this.Property(t => t.SC_OFF_DOWN).HasColumnName("SC_OFF_DOWN");
            this.Property(t => t.SC_MEMBER_PRICE_OFF).HasColumnName("SC_MEMBER_PRICE_OFF");
            this.Property(t => t.SC_NOT_MEMBER_PRICE_OFF).HasColumnName("SC_NOT_MEMBER_PRICE_OFF");
            this.Property(t => t.PRO_RULE_MEMBER_NET_SID).HasColumnName("PRO_RULE_MEMBER_NET_SID");
            this.Property(t => t.SC_NET_PRICE_OFF).HasColumnName("SC_NET_PRICE_OFF");
            this.Property(t => t.SC_NET_OFF_UP).HasColumnName("SC_NET_OFF_UP");
            this.Property(t => t.SC_NET_OFF_DOWN).HasColumnName("SC_NET_OFF_DOWN");

            // Relationships
            this.HasOptional(t => t.SHOP_INFO)
                .WithMany(t => t.SALE_CODE)
                .HasForeignKey(d => d.SHOP_SID);

        }
    }
}
