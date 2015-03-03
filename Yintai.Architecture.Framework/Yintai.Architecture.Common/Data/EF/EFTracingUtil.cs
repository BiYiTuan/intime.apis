using EFCachingProvider;
using EFProviderWrapperToolkit;
using EFTracingProvider;
using System;
using System.Configuration;
using System.Data.Common;
using System.Linq;

namespace Yintai.Architecture.Common.Data.EF
{
    /// <summary>
    /// CLR Version: 4.0.30319.269
    /// NameSpace: Yintai.Architecture.PMS.Data
    /// FileName: EFTracingUtil
    ///
    /// Created at 10/11/2012 11:39:50 AM
    /// Description: 
    /// </summary>
    public class EFTracingUtil
    {
        public static DbConnection GetConnection(string nameOrConnectionString)
        {
            try
            {
                // this only supports entity connection strings http://msdn.microsoft.com/en-us/library/cc716756.aspx 
                return EntityConnectionWrapperUtils.CreateEntityConnectionWithWrappers(nameOrConnectionString, "EFTracingProvider",
        "EFCachingProvider");
            }
            catch (ArgumentException ex)
            {
                if (nameOrConnectionString.Contains('='))
                {
                    nameOrConnectionString = nameOrConnectionString.Substring(nameOrConnectionString.IndexOf('=') + 1);
                }
                // an invalid entity connection string is assumed to be a normal connection string name or connection string (Code First) 
                ConnectionStringSettings connectionStringSetting =
                    ConfigurationManager.ConnectionStrings[nameOrConnectionString];
                string connectionString;
                string providerName;

                if (connectionStringSetting != null)
                {
                    connectionString = connectionStringSetting.ConnectionString;
                    providerName = connectionStringSetting.ProviderName;
                }
                else
                {
                    providerName = "System.Data.SqlClient";
                    connectionString = nameOrConnectionString;
                }
                var connect = EFConnectionWrapperFactory.GetConnection();

                var wrapperConnectionString =
    String.Format(@"wrappedProvider={0};{1}", providerName, connectionString);
                connect.ConnectionString = wrapperConnectionString;

                return connect;
            }
        }

        private static DbConnection CreateConnection(string connectionString, string providerInvariantName, params string[] wrapperProviderInvariantNames)
        {
            DbConnection connection = CreateTracingConnection(connectionString, providerInvariantName);



            //DbConnection storeConnection = null;// DbProviderFactories.GetFactory(providerInvariantName).CreateConnection();
            //storeConnection.ConnectionString = connectionString;//ecsb.ProviderConnectionString;
            var newEntityConnection = WrapConnection(connection, connectionString, providerInvariantName, wrapperProviderInvariantNames);

            return newEntityConnection;
        }

        /// <summary>
        /// Wraps the connection.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="wrapperProviderInvariantNames">The wrapper provider invariant names.</param>
        /// <returns>Wrapped connection.</returns>
        internal static DbConnection WrapConnection(DbConnection connection, string connectionString, string providerInvariantName, params string[] wrapperProviderInvariantNames)
        {
            foreach (string invariantName in wrapperProviderInvariantNames)
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(invariantName);
                var connectionWrapper = factory.CreateConnection();
                //connectionWrapper.ConnectionString = String.Format(@"wrappedProvider={0};{1}", providerInvariantName, connectionString);
                DbConnectionWrapper wrapper = (DbConnectionWrapper)connectionWrapper;
                wrapper.WrappedConnection = connection;
                connection = connectionWrapper;
            }

            return connection;
        }


        private static EFTracingConnection CreateTracingConnection(string connectionString, string providerInvariantName)
        {
            string wrapperConnectionString =
                String.Format(@"wrappedProvider={0};{1}", providerInvariantName, connectionString);

            var connection =
                new EFTracingConnection()
                {
                    ConnectionString = wrapperConnectionString
                };

            return connection;
        }
    }

    internal class EFConnectionWrapperFactory
    {
        private const string EfConnectionConfigKey = "efconnection";
        private const string EfCacheingConnection = "efCacheingConnection";
        private const string EfTracingConnection = "efTracingConnection";

        public static DbConnectionWrapper GetConnection()
        {
            var key = ConfigurationManager.AppSettings.AllKeys.Contains(EfConnectionConfigKey) ? ConfigurationManager.AppSettings[EfConnectionConfigKey] : String.Empty;

            if (String.IsNullOrWhiteSpace(key))
            {
                key = EfTracingConnection;
            }

            switch (key)
            {
                case EfCacheingConnection:
                    return new EFCachingConnection();
                default:
                    return new EFTracingConnection();
            }
        }
    }
}
