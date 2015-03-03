using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class Group_WeixinKeysEntityMap : EntityTypeConfiguration<Group_WeixinKeysEntity>
    {
        public Group_WeixinKeysEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.AppId)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.AppSecret)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PaySignKey)
                .IsRequired()
                .HasMaxLength(500);

            this.Property(t => t.ParterId)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.ParterKey)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.PaidLikeUrl)
                .HasMaxLength(500);

            this.Property(t => t.Outcome_OperatorId)
                .HasMaxLength(100);

            this.Property(t => t.Outcome_OperatorPwd)
                .HasMaxLength(100);

            this.Property(t => t.Outcome_ParterId)
                .HasMaxLength(100);

            this.Property(t => t.Outcome_ParterKey)
                .HasMaxLength(100);

            this.Property(t => t.Outcome_CARelativeFilePath)
                .HasMaxLength(500);

            this.Property(t => t.Outcome_CertRelativeFilePath)
                .HasMaxLength(500);

            this.Property(t => t.Outcome_CertFilePwd)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Group_WeixinKeys");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.GroupId).HasColumnName("GroupId");
            this.Property(t => t.AppId).HasColumnName("AppId");
            this.Property(t => t.AppSecret).HasColumnName("AppSecret");
            this.Property(t => t.PaySignKey).HasColumnName("PaySignKey");
            this.Property(t => t.ParterId).HasColumnName("ParterId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.CreateDate).HasColumnName("CreateDate");
            this.Property(t => t.CreateUser).HasColumnName("CreateUser");
            this.Property(t => t.ParterKey).HasColumnName("ParterKey");
            this.Property(t => t.PaidLikeUrl).HasColumnName("PaidLikeUrl");
            this.Property(t => t.Outcome_OperatorId).HasColumnName("Outcome_OperatorId");
            this.Property(t => t.Outcome_OperatorPwd).HasColumnName("Outcome_OperatorPwd");
            this.Property(t => t.Outcome_ParterId).HasColumnName("Outcome_ParterId");
            this.Property(t => t.Outcome_ParterKey).HasColumnName("Outcome_ParterKey");
            this.Property(t => t.Outcome_CARelativeFilePath).HasColumnName("Outcome_CARelativeFilePath");
            this.Property(t => t.Outcome_CertRelativeFilePath).HasColumnName("Outcome_CertRelativeFilePath");
            this.Property(t => t.Outcome_CertFilePwd).HasColumnName("Outcome_CertFilePwd");
		Init();
        }

		partial void Init();
    }
}
