using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class ShippingRuleFixEntityMap : EntityTypeConfiguration<ShippingRuleFixEntity>
    {
        public ShippingRuleFixEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("ShippingRuleFix");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RuleId).HasColumnName("RuleId");
            this.Property(t => t.Amount).HasColumnName("Amount");
		Init();
        }

		partial void Init();
    }
}
