using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using com.intime.fashion.data.erp.Models.Mapping;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace com.intime.fashion.data.erp.Models
{
    public partial class ErpContext : DbContext
    {
        static ErpContext()
        {
            Database.SetInitializer<ErpContext>(null);
        }

        public ErpContext()
            : base("Name=ErpContext")
        {
        }

        public DbSet<BRAND> BRANDs { get; set; }
        public DbSet<PRO_CLASS_DICT> PRO_CLASS_DICT { get; set; }
        public DbSet<PRO_PICTURE> PRO_PICTURE { get; set; }
        public DbSet<SALE_CODE> SALE_CODE { get; set; }
        public DbSet<SHOP_INFO> SHOP_INFO { get; set; }
        public DbSet<SUPPLY_MIN_PRICE> SUPPLY_MIN_PRICE { get; set; }
        public DbSet<SUPPLY_MIN_PRICE_MX> SUPPLY_MIN_PRICE_MX { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Configuration.AutoDetectChangesEnabled = false;
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new BRANDMap());
            modelBuilder.Configurations.Add(new PRO_CLASS_DICTMap());
            modelBuilder.Configurations.Add(new PRO_PICTUREMap());
            modelBuilder.Configurations.Add(new SALE_CODEMap());
            modelBuilder.Configurations.Add(new SHOP_INFOMap());
            modelBuilder.Configurations.Add(new SUPPLY_MIN_PRICEMap());
            modelBuilder.Configurations.Add(new SUPPLY_MIN_PRICE_MXMap());
        }

       
    }
}
