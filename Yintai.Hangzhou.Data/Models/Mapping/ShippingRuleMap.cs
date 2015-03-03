using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class ShippingRuleEntityMap : EntityTypeConfiguration<ShippingRuleEntity>
    {
        public ShippingRuleEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("ShippingRule");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RuleType).HasColumnName("RuleType");
            this.Property(t => t.MatchMethod).HasColumnName("MatchMethod");
            this.Property(t => t.FromDate).HasColumnName("FromDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.MatchId).HasColumnName("MatchId");
		Init();
        }

		partial void Init();
    }
}
