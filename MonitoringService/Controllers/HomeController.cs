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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendStudentNotification()
        {
            IResourceInfoService resourceService = new ResourceService();
            var messages = resourceService.GetResourceDetails();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }

    }
}