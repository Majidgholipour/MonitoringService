using MonitoringService.DTOs;
using MonitoringService.IServices;
using MonitoringService.Repository;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace MonitoringService.Services
{
    public class ResourceService : IResourceInfoService
    {
        public List<ResourceDTO> GetResourceDetails()
        {
            return ResourceRepository.GetResourceRecords();
        }
    }
}