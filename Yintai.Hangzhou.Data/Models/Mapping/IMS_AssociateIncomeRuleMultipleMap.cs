using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class IMS_AssociateIncomeRuleMultipleEntityMap : EntityTypeConfiguration<IMS_AssociateIncomeRuleMultipleEntity>
    {
        public IMS_AssociateIncomeRuleMultipleEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("IMS_AssociateIncomeRuleMultiple");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RuleId).HasColumnName("RuleId");
            this.Property(t => t.EffectTimeFrom).HasColumnName("EffectTimeFrom");
            this.Property(t => t.EffectTimeTo).HasColumnName("EffectTimeTo");
            this.Property(t => t.Multiple).HasColumnName("Multiple");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
		Init();
        }

		partial void Init();
    }
}
