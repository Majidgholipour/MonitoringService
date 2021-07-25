using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonitoringService.DTOs
{
    public class ResourceDTO
    {
        public DateTime InsertedDateTime { get; set; }
        ///cpu
        public float UsagePresent { get; set; }
        public int CountOfCore { get; set; }
        public float Speed { get; set; }
        public int CountOfProcessoer { get; set; }
        //Disk
        public long TotalSize { get; set; }
        public long TotalFreesize { get; set; }
        public long AvailableFreeSpace { get; set; }
        public string DrivesInfo { get; set; }

        //network
        public string AdaptorName { get; set; }
        public string DomainName { get; set; }
        public string ConnectionType { get; set; }
        public string IPv4Address { get; set; }
        public string IPv6Address { get; set; }
        public decimal Send { get; set; }
        public decimal Receive { get; set; }

        //Memory
        public float Size { get; set; }
        public float Usage { get; set; }
        public float InUse { get; set; }
        public float Avialible { get; set; }
        public float Cached { get; set; }
        public float Commited { get; set; }

        //Server
        public string Domain { get; set; }
        public string LocalIP { get; set; }
        public string HostName { get; set; }
        public string ServerName { get; set; }
    }
}