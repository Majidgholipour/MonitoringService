using MonitoringService.DTOs;
using System.Collections.Generic;

namespace MonitoringService.IServices
{
    public interface IApplicationLogService
    {
        List<ApplicationLogDTO> GetApplicationServiceDetails();
    }
}