
using MonitoringService.DTOs;
using MonitoringService.IServices;
using MonitoringService.Repository;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace MonitoringService.Services
{
    public class ApplicationLogService : IApplicationLogService
    {
        public List<ApplicationLogDTO> GetApplicationServiceDetails()
        {
            return ApplicationLogRepository.GetApplicationLogRecords();
        }
    }
}