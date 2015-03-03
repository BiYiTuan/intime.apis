using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.intime.fashion.data.erp.Models.Mapping
{
    public class SUPPLY_MIN_PRICE_MXMap : EntityTypeConfiguration<SUPPLY_MIN_PRICE_MX>
    {
        public SUPPLY_MIN_PRICE_MXMap()
        {
            // Primary Key
            this.HasKey(t => t.SID);

            // Properties
            this.Property(t => t.SID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PRO_COLOR)
                .HasMaxLength(100);

            this.Property(t => t.PRO_STAN_NAME)
                .HasMaxLength(100);

            this.Property(t => t.OPT_REAL_NAME)
                .HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("SUPPLY_MIN_PRICE_MX", "YINTAIWEB");
            this.Property(t => t.SID).HasColumnName("SID");
            this.Property(t => t.SUPPLY_MIN_PRICE_SID).HasColumnName("SUPPLY_MIN_PRICE_SID");
            this.Property(t => t.PRODUCT_SID).HasColumnName("PRODUCT_SID");
            this.Property(t => t.PRO_DETAIL_SID).HasColumnName("PRO_DETAIL_SID");
            this.Property(t => t.PRO_STOCK_SUM).HasColumnName("PRO_STOCK_SUM");
            this.Property(t => t.PRO_ACTIVE_BIT).HasColumnName("PRO_ACTIVE_BIT");
            this.Property(t => t.VERSION).HasColumnName("VERSION");
            this.Property(t => t.PRO_COLOR).HasColumnName("PRO_COLOR");
            this.Property(t => t.PRO_STAN_NAME).HasColumnName("PRO_STAN_NAME");
            this.Property(t => t.PRO_COLOR_SID).HasColumnName("PRO_COLOR_SID");
            this.Property(t => t.PRO_STAN_SID).HasColumnName("PRO_STAN_SID");
            this.Property(t => t.OPT_USER_SID).HasColumnName("OPT_USER_SID");
            this.Property(t => t.OPT_REAL_NAME).HasColumnName("OPT_REAL_NAME");
            this.Property(t => t.OPT_UPDATE_TIME).HasColumnName("OPT_UPDATE_TIME");

            // Relationships
            this.HasRequired(t => t.SUPPLY_MIN_PRICE)
                .WithMany(t => t.SUPPLY_MIN_PRICE_MX)
                .HasForeignKey(d => d.SUPPLY_MIN_PRICE_SID);

        }
    }
}
