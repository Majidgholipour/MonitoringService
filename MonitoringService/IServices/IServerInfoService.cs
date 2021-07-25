using MonitoringService.DTOs;
using System.Collections.Generic;

namespace MonitoringService.IServices
{
    public interface IServerInfoService
    {
        List<ServerInfoDTO> GetServerInfoDetails();
    }
}