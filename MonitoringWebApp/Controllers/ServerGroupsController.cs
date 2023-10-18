using Monitoring.Common;
using Monitoring.Common.CommonModels;
using MonitoringWebService.BAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MonitoringWebApp.Controllers
{
    public class ServerGroupsController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateGroups()
        {
            return View();
        }

        public JsonResult GetGroups()
        {
            MonitoringBLL monitoringBLL = new MonitoringBLL();
            var result = monitoringBLL.GetGroups();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditGroups(int Id)
        {
            MonitoringBLL monitoringBLL = new MonitoringBLL();
            var result = monitoringBLL.GetGroupsbyId(Id);

            return View(result);
        }

        public ActionResult UpdateGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            MonitoringBLL monitoringBLL = new MonitoringBLL();
            monitoringBLL.updateGroups(objDevicesViewModel);

            return RedirectToAction("Index");
        }

        public ActionResult SaveGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            MonitoringBLL monitoringBLL = new MonitoringBLL();
            monitoringBLL.CreateGroups(objDevicesViewModel);

            return RedirectToAction("Index");
        }

        public ActionResult UpdateDevicestoGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            return View();
        }

        public ActionResult DeleteGroup(int Id)
        {
            MonitoringBLL monitoringBLL = new MonitoringBLL();
            monitoringBLL.removeGroups(Id);

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SaveDeviceManager(ServerGroupViewModel objtblServerGroups)
        {
            MonitoringBLL monitoringBLL = new MonitoringBLL();
            monitoringBLL.MapServerstoGroup(objtblServerGroups);
            return RedirectToAction("Index");
        }

        public ActionResult DeviceManager(int Id)
        {
            MonitoringBLL monitoringBLL = new MonitoringBLL();

            ServerGroupViewModel objServerGroupViewModel = new ServerGroupViewModel();

            objServerGroupViewModel = monitoringBLL.GetServerGroupById(Id);

            return View(objServerGroupViewModel);
        }

    }
}