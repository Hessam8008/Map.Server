using System;
using System.Collections.Generic;
using System.Text;

namespace Map.Modules.Teltonika
{
    public interface IConfig
    {
        public string ConnectionString { get; }
    }
}
