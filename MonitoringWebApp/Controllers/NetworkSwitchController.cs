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
    public class NetworkSwitchController : Controller
    {
        NetworkSwitchBLL objNetworkSwitchBLL = new NetworkSwitchBLL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateGroups()
        {
            return View();
        }

        public JsonResult GetNetworkSwitchGroups()
        {
            var result = objNetworkSwitchBLL.GetNetworkSwitchGroups();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditGroups(int Id)
        {
            var result = objNetworkSwitchBLL.GetNetworkSwitchGroupsId(Id);
            return View(result);
        }

        public ActionResult UpdateGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            objNetworkSwitchBLL.UpdateNetworkSwitchGroups(objDevicesViewModel);

            return RedirectToAction("Index");
        }

        public ActionResult SaveGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            objDevicesViewModel.DeviceTypeId = (int)DeviceTypes.NetworkSwitches;
            objNetworkSwitchBLL.CreateNetworkSwitchGroups(objDevicesViewModel);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateDevicestoGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            return View();
        }

        public ActionResult DeleteNetworkSwitchGroups(int Id)
        {
            objNetworkSwitchBLL.DeleteNetworkSwitchGroups(Id);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SaveDeviceManager(NetworkSwitchGroupViewModel objNetworkSwitchGroupViewModel)
        {
            objNetworkSwitchBLL.MapNetworkSwitchGroups(objNetworkSwitchGroupViewModel);
            return RedirectToAction("Index");
        }

        public ActionResult DeviceManager(int Id)
        {
            NetworkSwitchGroupViewModel objNetworkSwitchGroupViewModel = new NetworkSwitchGroupViewModel();

            objNetworkSwitchGroupViewModel = objNetworkSwitchBLL.GetNetworkSwitchGroupDetailsById(Id);

            return View(objNetworkSwitchGroupViewModel);
        }

    }
}