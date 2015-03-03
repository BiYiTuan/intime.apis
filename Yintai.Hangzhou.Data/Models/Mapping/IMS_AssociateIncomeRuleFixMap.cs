using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class IMS_AssociateIncomeRuleFixEntityMap : EntityTypeConfiguration<IMS_AssociateIncomeRuleFixEntity>
    {
        public IMS_AssociateIncomeRuleFixEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("IMS_AssociateIncomeRuleFix");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RuleId).HasColumnName("RuleId");
            this.Property(t => t.FixAmount).HasColumnName("FixAmount");
		Init();
        }

		partial void Init();
    }
}
