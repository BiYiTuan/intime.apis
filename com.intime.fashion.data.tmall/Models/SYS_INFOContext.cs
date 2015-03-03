using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using com.intime.fashion.data.tmall.Models.Mapping;

namespace com.intime.fashion.data.tmall.Models
{
    public partial class SYS_INFOContext : DbContext
    {
        static SYS_INFOContext()
        {
            Database.SetInitializer<SYS_INFOContext>(null);
        }

        public SYS_INFOContext()
            : base("Name=SYS_INFOContext")
        {
        }

        public DbSet<JDP_TB_ITEM> JDP_TB_ITEM { get; set; }
        public DbSet<JDP_TB_TRADE> JDP_TB_TRADE { get; set; }
        public DbSet<LogisticsAddressMapping> LogisticsAddressMappings { get; set; } 
        public DbSet<OrderSync> OrderSyncs { get; set; }
        public DbSet<OrderSynchErrorLog> OrderSynchErrorLogs { get; set; }
        public DbSet<ShipViaMapping> ShipViaMappings { get; set; }
        public DbSet<SubOrder> SubOrders { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new JDP_TB_ITEMMap());
            modelBuilder.Configurations.Add(new JDP_TB_TRADEMap());
            modelBuilder.Configurations.Add(new LogisticsAddressMappingMap());
            modelBuilder.Configurations.Add(new OrderSyncMap());
            modelBuilder.Configurations.Add(new OrderSynchErrorLogMap());
            modelBuilder.Configurations.Add(new ShipViaMappingMap());
            modelBuilder.Configurations.Add(new SubOrderMap());
        }
    }
}
