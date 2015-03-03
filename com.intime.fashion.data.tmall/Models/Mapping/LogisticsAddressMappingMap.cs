using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace com.intime.fashion.data.tmall.Models.Mapping
{
    public class LogisticsAddressMappingMap : EntityTypeConfiguration<LogisticsAddressMapping>
    {
        public LogisticsAddressMappingMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("LogisticsAddressMapping");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.StoreId).HasColumnName("StoreId");
            this.Property(t => t.TmallAddressId).HasColumnName("TmallAddressId");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
        }
    }
}
