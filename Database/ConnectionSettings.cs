using System.Collections.Generic;
using LinqToDB.Configuration;
// ReSharper disable UnassignedGetOnlyAutoProperty

namespace gta_mp_database {
    /// <summary>
    /// Настройки подключения к БД
    /// </summary>
    public class ConnectionSettings : IConnectionStringSettings {
        public string ConnectionString { get; set; }
        public string Name { get; set; }
        public string ProviderName { get; set; }
        public bool IsGlobal => true;
    }

    public class AppSettings : ILinqToDBSettings {
        public IEnumerable<IDataProviderSettings> DataProviders { get; }
        public string DefaultConfiguration => "SqlServer";
        public string DefaultDataProvider => "SqlServer";

        public IEnumerable<IConnectionStringSettings> ConnectionStrings {
            get {
                yield return new ConnectionSettings {
                    Name = "GtaVServer",
                    ProviderName = "SqlServer",
                    //ConnectionString = "Server=localhost;Database=gta-mp-server;User Id=sa;Password=Ab933095;"
                    ConnectionString = @"Server=localhost\SQLEXPRESS;Database=gta-mp-server;User Id=sa;Password=Gta5Server;"
                };
            }
        }
    }
}