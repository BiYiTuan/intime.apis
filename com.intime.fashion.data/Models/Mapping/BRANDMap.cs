using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.intime.fashion.data.erp.Models.Mapping
{
    public class BRANDMap : EntityTypeConfiguration<BRAND>
    {
        public BRANDMap()
        {
            // Primary Key
            this.HasKey(t => t.SID);

            // Properties
            this.Property(t => t.SID)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.BRAND_NAME)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.PICTURE_URL)
                .HasMaxLength(200);

            this.Property(t => t.BRAND_NAME_SECOND)
                .HasMaxLength(80);

            this.Property(t => t.BRANDNO)
                .HasMaxLength(40);

            this.Property(t => t.BRANDCORP)
                .HasMaxLength(100);

            this.Property(t => t.BRANDPIC1)
                .HasMaxLength(400);

            this.Property(t => t.BRANDPIC2)
                .HasMaxLength(400);

            this.Property(t => t.SPELL)
                .HasMaxLength(100);

            this.Property(t => t.OPT_REAL_NAME)
                .HasMaxLength(40);

            this.Property(t => t.SIZECOMPARE)
                .HasMaxLength(50);

            this.Property(t => t.BRAND_STORY)
                .HasMaxLength(1000);

            this.Property(t => t.BRAND_NAME_IMPORT)
                .HasMaxLength(80);

            this.Property(t => t.BRAND_NAME_ALL)
                .HasMaxLength(4000);

            this.Property(t => t.BRAND_NAME_SECOND_ALL)
                .HasMaxLength(4000);

            this.Property(t => t.BRAND_NAME_1)
                .HasMaxLength(80);

            this.Property(t => t.BRAND_NAME_SECOND_1)
                .HasMaxLength(80);

            this.Property(t => t.BRAND_NAME_2)
                .HasMaxLength(80);

            this.Property(t => t.BRAND_NAME_SECOND_2)
                .HasMaxLength(80);

            this.Property(t => t.BRAND_NAME_3)
                .HasMaxLength(80);

            this.Property(t => t.BRAND_NAME_SECOND_3)
                .HasMaxLength(80);

            this.Property(t => t.BRAND_NAME_4)
                .HasMaxLength(80);

            this.Property(t => t.BRAND_NAME_SECOND_4)
                .HasMaxLength(80);

            this.Property(t => t.BRAND_NAME_5)
                .HasMaxLength(80);

            this.Property(t => t.BRAND_NAME_SECOND_5)
                .HasMaxLength(80);

            this.Property(t => t.BRAND_NAME_6)
                .HasMaxLength(80);

            this.Property(t => t.BRAND_NAME_SECOND_6)
                .HasMaxLength(80);

            this.Property(t => t.BRAND_NAME_7)
                .HasMaxLength(80);

            this.Property(t => t.BRAND_NAME_SECOND_7)
                .HasMaxLength(80);

            this.Property(t => t.SEO_TITLE)
                .HasMaxLength(1000);

            this.Property(t => t.SEO_KEYWORD)
                .HasMaxLength(1000);

            this.Property(t => t.SEO_DESCRIPTION)
                .HasMaxLength(1000);

            // Table & Column Mappings
            this.ToTable("BRAND", "YINTAIWEB");
            this.Property(t => t.SID).HasColumnName("SID");
            this.Property(t => t.BRAND_NAME).HasColumnName("BRAND_NAME");
            this.Property(t => t.PICTURE_URL).HasColumnName("PICTURE_URL");
            this.Property(t => t.BRAND_NAME_SECOND).HasColumnName("BRAND_NAME_SECOND");
            this.Property(t => t.BRAND_ACTIVE_BIT).HasColumnName("BRAND_ACTIVE_BIT");
            this.Property(t => t.BRANDNO).HasColumnName("BRANDNO");
            this.Property(t => t.BRANDCORP).HasColumnName("BRANDCORP");
            this.Property(t => t.BRANDPIC1).HasColumnName("BRANDPIC1");
            this.Property(t => t.BRANDPIC2).HasColumnName("BRANDPIC2");
            this.Property(t => t.PHOTO_BLACKLIST_BIT).HasColumnName("PHOTO_BLACKLIST_BIT");
            this.Property(t => t.PARENT_SID).HasColumnName("PARENT_SID");
            this.Property(t => t.END_BIT).HasColumnName("END_BIT");
            this.Property(t => t.SPELL).HasColumnName("SPELL");
            this.Property(t => t.OPT_USER_SID).HasColumnName("OPT_USER_SID");
            this.Property(t => t.OPT_REAL_NAME).HasColumnName("OPT_REAL_NAME");
            this.Property(t => t.OPT_UPDATE_TIME).HasColumnName("OPT_UPDATE_TIME");
            this.Property(t => t.BRAND_ROOT_SID).HasColumnName("BRAND_ROOT_SID");
            this.Property(t => t.BRAND_LEVEL).HasColumnName("BRAND_LEVEL");
            this.Property(t => t.BRAND_WEIGHT).HasColumnName("BRAND_WEIGHT");
            this.Property(t => t.VERSION).HasColumnName("VERSION");
            this.Property(t => t.SIZECOMPARE).HasColumnName("SIZECOMPARE");
            this.Property(t => t.BRAND_STORY).HasColumnName("BRAND_STORY");
            this.Property(t => t.BRAND_NAME_IMPORT).HasColumnName("BRAND_NAME_IMPORT");
            this.Property(t => t.AREA_SID).HasColumnName("AREA_SID");
            this.Property(t => t.BRAND_GROUP_NUM).HasColumnName("BRAND_GROUP_NUM");
            this.Property(t => t.DISPLAY_NUM).HasColumnName("DISPLAY_NUM");
            this.Property(t => t.BRAND_NAME_ALL).HasColumnName("BRAND_NAME_ALL");
            this.Property(t => t.BRAND_NAME_SECOND_ALL).HasColumnName("BRAND_NAME_SECOND_ALL");
            this.Property(t => t.BRAND_NAME_1).HasColumnName("BRAND_NAME_1");
            this.Property(t => t.BRAND_NAME_SECOND_1).HasColumnName("BRAND_NAME_SECOND_1");
            this.Property(t => t.BRAND_NAME_2).HasColumnName("BRAND_NAME_2");
            this.Property(t => t.BRAND_NAME_SECOND_2).HasColumnName("BRAND_NAME_SECOND_2");
            this.Property(t => t.BRAND_NAME_3).HasColumnName("BRAND_NAME_3");
            this.Property(t => t.BRAND_NAME_SECOND_3).HasColumnName("BRAND_NAME_SECOND_3");
            this.Property(t => t.BRAND_NAME_4).HasColumnName("BRAND_NAME_4");
            this.Property(t => t.BRAND_NAME_SECOND_4).HasColumnName("BRAND_NAME_SECOND_4");
            this.Property(t => t.BRAND_NAME_5).HasColumnName("BRAND_NAME_5");
            this.Property(t => t.BRAND_NAME_SECOND_5).HasColumnName("BRAND_NAME_SECOND_5");
            this.Property(t => t.BRAND_NAME_6).HasColumnName("BRAND_NAME_6");
            this.Property(t => t.BRAND_NAME_SECOND_6).HasColumnName("BRAND_NAME_SECOND_6");
            this.Property(t => t.BRAND_NAME_7).HasColumnName("BRAND_NAME_7");
            this.Property(t => t.BRAND_NAME_SECOND_7).HasColumnName("BRAND_NAME_SECOND_7");
            this.Property(t => t.SEO_TITLE).HasColumnName("SEO_TITLE");
            this.Property(t => t.SEO_KEYWORD).HasColumnName("SEO_KEYWORD");
            this.Property(t => t.SEO_DESCRIPTION).HasColumnName("SEO_DESCRIPTION");
        }
    }
}
