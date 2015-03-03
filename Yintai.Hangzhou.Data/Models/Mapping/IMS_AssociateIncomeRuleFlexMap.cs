using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class IMS_AssociateIncomeRuleFlexEntityMap : EntityTypeConfiguration<IMS_AssociateIncomeRuleFlexEntity>
    {
        public IMS_AssociateIncomeRuleFlexEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("IMS_AssociateIncomeRuleFlex");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RuleId).HasColumnName("RuleId");
            this.Property(t => t.BenchFrom).HasColumnName("BenchFrom");
            this.Property(t => t.BenchTo).HasColumnName("BenchTo");
            this.Property(t => t.Percentage).HasColumnName("Percentage");
		Init();
        }

		partial void Init();
    }
}
