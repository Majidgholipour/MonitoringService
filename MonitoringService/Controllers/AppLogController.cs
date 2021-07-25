using MonitoringService.IServices;
using MonitoringService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MonitoringService.Controllers
{
    public class AppLogController : Controller
    {
        // GET: AppLog
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendNotification()
        {
            IApplicationLogService service = new ApplicationLogService();
            var messages = service.GetApplicationServiceDetails();
            return Json(messages, JsonRequestBehavior.AllowGet);
        }
    }
}