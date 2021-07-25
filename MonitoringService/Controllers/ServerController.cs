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
    public class ServerController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

      
        public ActionResult SendNotification()
        {
            IServerInfoService service = new ServerInfoService();
            var messages = service.GetServerInfoDetails();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }


    }
}