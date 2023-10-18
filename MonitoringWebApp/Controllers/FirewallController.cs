using Monitoring.Common;
using Monitoring.Common.CommonModels;
using Monitoring.DataAccess.BAL;
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
    public class FirewallController : Controller
    {
        FirewallBLL objFirewallBLL = new FirewallBLL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateGroups()
        {
            return View();
        }

        public JsonResult GetFirewallGroups()
        {
            var result = objFirewallBLL.GetFirewallGroups();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditGroups(int Id)
        {
            var result = objFirewallBLL.GetFirewallGroupsId(Id);
            return View(result);
        }

        public ActionResult UpdateGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            objFirewallBLL.UpdateFirewallGroups(objDevicesViewModel);

            return RedirectToAction("Index");
        }

        public ActionResult SaveGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            objDevicesViewModel.DeviceTypeId = (int)DeviceTypes.Firewalls;
            objFirewallBLL.CreateFirewallGroups(objDevicesViewModel);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateDevicestoGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            return View();
        }

        public ActionResult DeleteAccessPointGroups(int Id)
        {
            objFirewallBLL.DeleteFirewallGroups(Id);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SaveDeviceManager(FirewallGroupViewModel objFirewallGroupViewModel)
        {
            objFirewallBLL.MapFireWallDevicetoGroup(objFirewallGroupViewModel);
            return RedirectToAction("Index");
        }

        //public ActionResult DeviceManager(int Id)
        //{
        //    OtherDeviceGroupViewModel objOtherDeviceGroupViewModel = new OtherDeviceGroupViewModel();

        //    objOtherDeviceGroupViewModel = objAccessPointsBLL.GetOtherDeviceGroupDetailsById(Id);

        //    return View(objOtherDeviceGroupViewModel);
        //}

    }
}