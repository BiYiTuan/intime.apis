using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class CardBlackEntityMap : EntityTypeConfiguration<CardBlackEntity>
    {
        public CardBlackEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CardNo)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("CardBlack");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CardNo).HasColumnName("CardNo");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
		Init();
        }

		partial void Init();
    }
}
