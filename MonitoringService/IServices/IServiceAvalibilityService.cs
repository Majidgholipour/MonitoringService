using MonitoringService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonitoringService.Models
{
    public interface IServiceAvalibilityService
    {
        List<ServiceAvalibilityDTO> GetServiceAvalibilityDetails();
    }
}