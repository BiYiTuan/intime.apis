using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class JobSuccessHistoryEntityMap : EntityTypeConfiguration<JobSuccessHistoryEntity>
    {
        public JobSuccessHistoryEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("JobSuccessHistory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.JobId).HasColumnName("JobId");
            this.Property(t => t.JobType).HasColumnName("JobType");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
		Init();
        }

		partial void Init();
    }
}
