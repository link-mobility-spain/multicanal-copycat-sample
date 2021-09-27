using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Link.Multicanal.WorkerService.Config
{
    public class ConnectionString : Dictionary<string, string> { }
    public class AppConfig
    {
        public string ENV { get; set; }
        public string ApplicationName { get; set; }
        public MulticanalAPIConfig MulticanalAPI { get; set; }

    }
}
