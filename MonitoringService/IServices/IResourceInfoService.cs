using MonitoringService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonitoringService.IServices
{
    public interface IResourceInfoService
    {
        List<ResourceDTO> GetResourceDetails();
    }
}