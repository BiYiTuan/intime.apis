using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class IMS_AssociateIncomeRuleFlattenEntityMap : EntityTypeConfiguration<IMS_AssociateIncomeRuleFlattenEntity>
    {
        public IMS_AssociateIncomeRuleFlattenEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("IMS_AssociateIncomeRuleFlatten");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RuleId).HasColumnName("RuleId");
            this.Property(t => t.Percentage).HasColumnName("Percentage");
		Init();
        }

		partial void Init();
    }
}
