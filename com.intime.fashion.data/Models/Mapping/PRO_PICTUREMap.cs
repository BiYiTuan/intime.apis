using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.intime.fashion.data.erp.Models.Mapping
{
    public class PRO_PICTUREMap : EntityTypeConfiguration<PRO_PICTURE>
    {
        public PRO_PICTUREMap()
        {
            // Primary Key
            this.HasKey(t => t.SID);

            // Properties
            this.Property(t => t.SID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PRO_PICT_NAME)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PRO_PICT_DIR)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.OPT_REAL_NAME)
                .HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("PRO_PICTURE", "YINTAIWEB");
            this.Property(t => t.SID).HasColumnName("SID");
            this.Property(t => t.PRODUCT_SID).HasColumnName("PRODUCT_SID");
            this.Property(t => t.PRO_PICT_NAME).HasColumnName("PRO_PICT_NAME");
            this.Property(t => t.PRO_PICT_DIR).HasColumnName("PRO_PICT_DIR");
            this.Property(t => t.PRO_PICT_ORDER).HasColumnName("PRO_PICT_ORDER");
            this.Property(t => t.PICTURE_MODEL_BIT).HasColumnName("PICTURE_MODEL_BIT");
            this.Property(t => t.PICTURE_MAST_BIT).HasColumnName("PICTURE_MAST_BIT");
            this.Property(t => t.PRO_COLOR_SID).HasColumnName("PRO_COLOR_SID");
            this.Property(t => t.PRO_PICTURE_SIZE_SID).HasColumnName("PRO_PICTURE_SIZE_SID");
            this.Property(t => t.PRO_WRI_TIME).HasColumnName("PRO_WRI_TIME");
            this.Property(t => t.OPT_USER_SID).HasColumnName("OPT_USER_SID");
            this.Property(t => t.OPT_REAL_NAME).HasColumnName("OPT_REAL_NAME");
            this.Property(t => t.OPT_UPDATE_TIME).HasColumnName("OPT_UPDATE_TIME");
            this.Property(t => t.VERSION).HasColumnName("VERSION");
            this.Property(t => t.MAIN_BIT).HasColumnName("MAIN_BIT");
            this.Property(t => t.UPLOAD_BIT).HasColumnName("UPLOAD_BIT");
            this.Property(t => t.CRC).HasColumnName("CRC");
            this.Property(t => t.DELETE_BIT).HasColumnName("DELETE_BIT");
        }
    }
}
