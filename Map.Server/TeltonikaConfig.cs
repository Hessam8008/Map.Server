using System;
using System.Collections.Generic;
using System.Text;
using Map.Modules.Teltonika;

namespace Map.Server
{
    class TeltonikaConfig:IConfig
    {
        public string ConnectionString { get; }

        public TeltonikaConfig(string connectionString)
        {
            ConnectionString = connectionString;
        }
    }
}
