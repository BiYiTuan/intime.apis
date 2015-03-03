using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class TimeSeedEntityMap : EntityTypeConfiguration<TimeSeedEntity>
    {
        public TimeSeedEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.KeySeed)
                .IsRequired()
                .HasMaxLength(32);

            // Table & Column Mappings
            this.ToTable("TimeSeed");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Year).HasColumnName("Year");
            this.Property(t => t.Month).HasColumnName("Month");
            this.Property(t => t.Day).HasColumnName("Day");
            this.Property(t => t.Hour).HasColumnName("Hour");
            this.Property(t => t.Seed).HasColumnName("Seed");
            this.Property(t => t.KeySeed).HasColumnName("KeySeed");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
		Init();
        }

		partial void Init();
    }
}
