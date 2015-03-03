using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class IMS_AssociateIncomeRuleEntityMap : EntityTypeConfiguration<IMS_AssociateIncomeRuleEntity>
    {
        public IMS_AssociateIncomeRuleEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("IMS_AssociateIncomeRule");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.FromDate).HasColumnName("FromDate");
            this.Property(t => t.EndDate).HasColumnName("EndDate");
            this.Property(t => t.RuleType).HasColumnName("RuleType");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.ComputeType).HasColumnName("ComputeType");
		Init();
        }

		partial void Init();
    }
}
