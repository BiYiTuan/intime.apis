using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.intime.fashion.data.tmall.Models.Mapping
{
    public class ShipViaMappingMap : EntityTypeConfiguration<ShipViaMapping>
    {
        public ShipViaMappingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CompanyCode)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Channel)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.CompanyName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ShipViaMapping");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ImsShipViaId).HasColumnName("ImsShipViaId");
            this.Property(t => t.CompanyCode).HasColumnName("CompanyCode");
            this.Property(t => t.Channel).HasColumnName("Channel");
            this.Property(t => t.CompanyName).HasColumnName("CompanyName");
        }
    }
}
