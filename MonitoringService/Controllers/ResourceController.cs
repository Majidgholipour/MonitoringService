using MonitoringService.IServices;
using MonitoringService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MonitoringService.Controllers
{
    public class ResourceController : Controller
    {
        // GET: Resource
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendNotification()
        {
            IResourceInfoService resourceService = new ResourceService();
            var messages = resourceService.GetResourceDetails();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
    }
}