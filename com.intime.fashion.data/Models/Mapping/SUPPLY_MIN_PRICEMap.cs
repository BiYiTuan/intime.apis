using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.intime.fashion.data.erp.Models.Mapping
{
    public class SUPPLY_MIN_PRICEMap : EntityTypeConfiguration<SUPPLY_MIN_PRICE>
    {
        public SUPPLY_MIN_PRICEMap()
        {
            // Primary Key
            this.HasKey(t => t.SID);

            // Properties
            this.Property(t => t.SID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PRO_SKU)
                .HasMaxLength(100);

            this.Property(t => t.PRO_SKU2)
                .HasMaxLength(100);

            this.Property(t => t.PROT_COLOR2)
                .HasMaxLength(20);

            this.Property(t => t.PRO_PICT_NAME)
                .HasMaxLength(100);

            this.Property(t => t.PRODUCT_NAME)
                .HasMaxLength(100);

            this.Property(t => t.OPT_REAL_NAME)
                .HasMaxLength(40);

            this.Property(t => t.PRO_DESC)
                .HasMaxLength(500);

            this.Property(t => t.BARCODE)
                .HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("SUPPLY_MIN_PRICE", "YINTAIWEB");
            this.Property(t => t.SID).HasColumnName("SID");
            this.Property(t => t.PRODUCT_SID).HasColumnName("PRODUCT_SID");
            this.Property(t => t.PRO_SELLING_TIME).HasColumnName("PRO_SELLING_TIME");
            this.Property(t => t.BRAND_SID).HasColumnName("BRAND_SID");
            this.Property(t => t.PROMOTION_PRICE).HasColumnName("PROMOTION_PRICE");
            this.Property(t => t.ORIGINAL_PRICE).HasColumnName("ORIGINAL_PRICE");
            this.Property(t => t.CURRENT_PRICE).HasColumnName("CURRENT_PRICE");
            this.Property(t => t.PROMOTION_BEGIN_TIME).HasColumnName("PROMOTION_BEGIN_TIME");
            this.Property(t => t.PROMOTION_END_TIME).HasColumnName("PROMOTION_END_TIME");
            this.Property(t => t.OFF_VALUE).HasColumnName("OFF_VALUE");
            this.Property(t => t.PRO_SUM).HasColumnName("PRO_SUM");
            this.Property(t => t.PRO_SKU).HasColumnName("PRO_SKU");
            this.Property(t => t.PRO_SKU2).HasColumnName("PRO_SKU2");
            this.Property(t => t.PROT_COLOR2).HasColumnName("PROT_COLOR2");
            this.Property(t => t.PRO_PICT_NAME).HasColumnName("PRO_PICT_NAME");
            this.Property(t => t.PRODUCT_NAME).HasColumnName("PRODUCT_NAME");
            this.Property(t => t.ORDER_WEIGHT).HasColumnName("ORDER_WEIGHT");
            this.Property(t => t.IS_NEW).HasColumnName("IS_NEW");
            this.Property(t => t.IS_HOT).HasColumnName("IS_HOT");
            this.Property(t => t.IS_PREVIEW).HasColumnName("IS_PREVIEW");
            this.Property(t => t.OPT_USER_SID).HasColumnName("OPT_USER_SID");
            this.Property(t => t.OPT_REAL_NAME).HasColumnName("OPT_REAL_NAME");
            this.Property(t => t.OPT_UPDATE_TIME).HasColumnName("OPT_UPDATE_TIME");
            this.Property(t => t.SALE_CODE_SID).HasColumnName("SALE_CODE_SID");
            this.Property(t => t.VERSION).HasColumnName("VERSION");
            this.Property(t => t.PRO_CLASS_SID).HasColumnName("PRO_CLASS_SID");
            this.Property(t => t.SHOP_SID).HasColumnName("SHOP_SID");
            this.Property(t => t.PRO_DESC).HasColumnName("PRO_DESC");
            this.Property(t => t.BARCODE).HasColumnName("BARCODE");
            this.Property(t => t.PRO_SELLING).HasColumnName("PRO_SELLING");
            this.Property(t => t.PRO_SELLING_DOWN_TIME).HasColumnName("PRO_SELLING_DOWN_TIME");

            // Relationships
            this.HasOptional(t => t.SALE_CODE)
                .WithMany(t => t.SUPPLY_MIN_PRICE)
                .HasForeignKey(d => d.SALE_CODE_SID);

        }
    }
}
