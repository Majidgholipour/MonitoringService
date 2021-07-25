using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonitoringService.DTOs
{
    public class ServerInfoDTO

    {
        public Guid Id { get; set; }
        public string Domain { get; set; }
        public string HostName { get; set; }
        public string LocalIP { get; set; }
        public string ServerName { get; set; }

    }
}