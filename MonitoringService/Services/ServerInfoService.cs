using MonitoringService.DTOs;
using MonitoringService.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonitoringService.Models
{
    public class ServerInfoService : IServerInfoService
    {
        public List<ServerInfoDTO> GetServerInfoDetails()
        {
            return ServerInfoRepository.GetServerInfoRecords();
        }
    }
}