using MonitoringService.DTOs;
using MonitoringService.Models;
using MonitoringService.Repository;
using System.Collections.Generic;

namespace MonitoringService.Services
{
    public class ServiceAvalibilityService : IServiceAvalibilityService
    {
        public List<ServiceAvalibilityDTO> GetServiceAvalibilityDetails()
        {
            return ServiceAvalibilityRepository.GetServiceAvalibilityRecords();
        }
    }
}