using MonitoringService.IServices;
using MonitoringService.Models;
using MonitoringService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MonitoringService.Controllers
{
    public class ServiceAvalibilityController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult SendNotification()
        {
            IServiceAvalibilityService service = new ServiceAvalibilityService();
            var messages = service.GetServiceAvalibilityDetails();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }


    }
}