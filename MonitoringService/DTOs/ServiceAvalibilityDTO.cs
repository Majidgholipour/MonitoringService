using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonitoringService.DTOs
{
    public class ServiceAvalibilityDTO
    {
        public string Domain { get; set; }
        public string LocalIP { get; set; }
        public string HostName { get; set; }
        public string ServerName { get; set; }
        public string GenerateDateTime { get; set; }
        public string serviceAvalibilityType { get; set; }
    }
}