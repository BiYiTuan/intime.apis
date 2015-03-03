using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.intime.fashion.data.erp.Models.Mapping
{
    public class SHOP_INFOMap : EntityTypeConfiguration<SHOP_INFO>
    {
        public SHOP_INFOMap()
        {
            // Primary Key
            this.HasKey(t => t.SID);

            // Properties
            this.Property(t => t.SID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.SHOP_NAME)
                .IsRequired()
                .HasMaxLength(40);

            this.Property(t => t.SHOP_LINKER)
                .HasMaxLength(40);

            this.Property(t => t.LINKER_PHONE)
                .HasMaxLength(30);

            this.Property(t => t.SHOP_NUM)
                .HasMaxLength(4);

            this.Property(t => t.SHOP_ADDR)
                .HasMaxLength(100);

            this.Property(t => t.REFUND_LINKER)
                .HasMaxLength(60);

            this.Property(t => t.REFUND_TEL)
                .HasMaxLength(60);

            this.Property(t => t.SPELL)
                .HasMaxLength(100);

            this.Property(t => t.OPT_REAL_NAME)
                .HasMaxLength(40);

            this.Property(t => t.WEB_SITE)
                .HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("SHOP_INFO", "YINTAIWEB");
            this.Property(t => t.SID).HasColumnName("SID");
            this.Property(t => t.SHOP_NAME).HasColumnName("SHOP_NAME");
            this.Property(t => t.SHOP_LINKER).HasColumnName("SHOP_LINKER");
            this.Property(t => t.LINKER_PHONE).HasColumnName("LINKER_PHONE");
            this.Property(t => t.SHOP_NUM).HasColumnName("SHOP_NUM");
            this.Property(t => t.NET_BIT).HasColumnName("NET_BIT");
            this.Property(t => t.SHOP_ADDR).HasColumnName("SHOP_ADDR");
            this.Property(t => t.POSTCODE).HasColumnName("POSTCODE");
            this.Property(t => t.REFUND_LINKER).HasColumnName("REFUND_LINKER");
            this.Property(t => t.REFUND_TEL).HasColumnName("REFUND_TEL");
            this.Property(t => t.SPELL).HasColumnName("SPELL");
            this.Property(t => t.OPT_USER_SID).HasColumnName("OPT_USER_SID");
            this.Property(t => t.OPT_REAL_NAME).HasColumnName("OPT_REAL_NAME");
            this.Property(t => t.OPT_UPDATE_TIME).HasColumnName("OPT_UPDATE_TIME");
            this.Property(t => t.POSSHOPCODE).HasColumnName("POSSHOPCODE");
            this.Property(t => t.PRO_CLASS_DICT_SID).HasColumnName("PRO_CLASS_DICT_SID");
            this.Property(t => t.PRICE_UP_BIT).HasColumnName("PRICE_UP_BIT");
            this.Property(t => t.PRINT_BARGAIN).HasColumnName("PRINT_BARGAIN");
            this.Property(t => t.WEB_SITE).HasColumnName("WEB_SITE");
        }
    }
}
