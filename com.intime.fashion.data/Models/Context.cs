using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using com.intime.fashion.data.erp.Models.Mapping;

namespace com.intime.fashion.data.erp.Models
{
    public partial class Context : DbContext
    {
        static Context()
        {
            Database.SetInitializer<Context>(null);
        }

        public Context()
            : base("Name=Context")
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
