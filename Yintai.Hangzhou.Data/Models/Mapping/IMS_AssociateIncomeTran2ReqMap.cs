using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Yintai.Hangzhou.Data.Models.Mapping
{
    public partial class IMS_AssociateIncomeTran2ReqEntityMap : EntityTypeConfiguration<IMS_AssociateIncomeTran2ReqEntity>
    {
        public IMS_AssociateIncomeTran2ReqEntityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("IMS_AssociateIncomeTran2Req");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FullPackageId).HasColumnName("FullPackageId");
            this.Property(t => t.RequestId).HasColumnName("RequestId");
		Init();
        }

		partial void Init();
    }
}
