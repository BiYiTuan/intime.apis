using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.intime.fashion.data.erp.Models.Mapping
{
    public class PRO_CLASS_DICTMap : EntityTypeConfiguration<PRO_CLASS_DICT>
    {
        public PRO_CLASS_DICTMap()
        {
            // Primary Key
            this.HasKey(t => t.SID);

            // Properties
            this.Property(t => t.SID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.PRO_CLASS_NUM)
                .IsRequired()
                .HasMaxLength(60);

            this.Property(t => t.PRO_CLASS_DESC)
                .HasMaxLength(100);

            this.Property(t => t.CLASS_MASTER_NUM)
                .HasMaxLength(60);

            this.Property(t => t.CLASS_NUM_1)
                .HasMaxLength(60);

            this.Property(t => t.CLASS_NUM_2)
                .HasMaxLength(60);

            this.Property(t => t.CLASS_NUM_3)
                .HasMaxLength(60);

            this.Property(t => t.CLASS_NUM_4)
                .HasMaxLength(60);

            this.Property(t => t.CLASS_NUM_5)
                .HasMaxLength(60);

            this.Property(t => t.CLASS_NUM_6)
                .HasMaxLength(60);

            this.Property(t => t.CLASS_NUM_7)
                .HasMaxLength(60);

            this.Property(t => t.OPT_REAL_NAME)
                .HasMaxLength(40);

            // Table & Column Mappings
            this.ToTable("PRO_CLASS_DICT", "YINTAIWEB");
            this.Property(t => t.SID).HasColumnName("SID");
            this.Property(t => t.PRO_CLASS_NUM).HasColumnName("PRO_CLASS_NUM");
            this.Property(t => t.PRO_CLASS_DESC).HasColumnName("PRO_CLASS_DESC");
            this.Property(t => t.CLASS_MASTER_SID).HasColumnName("CLASS_MASTER_SID");
            this.Property(t => t.PRO_BIT).HasColumnName("PRO_BIT");
            this.Property(t => t.PRO_LEVEL).HasColumnName("PRO_LEVEL");
            this.Property(t => t.CLASS_MASTER_NUM).HasColumnName("CLASS_MASTER_NUM");
            this.Property(t => t.CLASS_ID_1).HasColumnName("CLASS_ID_1");
            this.Property(t => t.CLASS_ID_2).HasColumnName("CLASS_ID_2");
            this.Property(t => t.CLASS_ID_3).HasColumnName("CLASS_ID_3");
            this.Property(t => t.PRO_CLASS_BIT).HasColumnName("PRO_CLASS_BIT");
            this.Property(t => t.CLASS_ID_4).HasColumnName("CLASS_ID_4");
            this.Property(t => t.CLASS_ID_5).HasColumnName("CLASS_ID_5");
            this.Property(t => t.CLASS_ID_6).HasColumnName("CLASS_ID_6");
            this.Property(t => t.CLASS_ID_7).HasColumnName("CLASS_ID_7");
            this.Property(t => t.CLASS_NUM_1).HasColumnName("CLASS_NUM_1");
            this.Property(t => t.CLASS_NUM_2).HasColumnName("CLASS_NUM_2");
            this.Property(t => t.CLASS_NUM_3).HasColumnName("CLASS_NUM_3");
            this.Property(t => t.CLASS_NUM_4).HasColumnName("CLASS_NUM_4");
            this.Property(t => t.CLASS_NUM_5).HasColumnName("CLASS_NUM_5");
            this.Property(t => t.CLASS_NUM_6).HasColumnName("CLASS_NUM_6");
            this.Property(t => t.CLASS_NUM_7).HasColumnName("CLASS_NUM_7");
            this.Property(t => t.VERSION).HasColumnName("VERSION");
            this.Property(t => t.OPT_USER_SID).HasColumnName("OPT_USER_SID");
            this.Property(t => t.OPT_REAL_NAME).HasColumnName("OPT_REAL_NAME");
            this.Property(t => t.OPT_UPDATE_TIME).HasColumnName("OPT_UPDATE_TIME");
        }
    }
}
