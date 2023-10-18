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
    public class OtherDeviceGroupController : Controller
    {
        OtherDevicesBLL otherDevicesBLL = new OtherDevicesBLL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateGroups()
        {
            return View();
        }

        public JsonResult GetOtherDeviceGroups()
        {
            var result = otherDevicesBLL.GetOtherDeviceGroups();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditGroups(int Id)
        {
            var result = otherDevicesBLL.GetOtherDeviceGroupsId(Id);
            return View(result);
        }

        public ActionResult UpdateGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            otherDevicesBLL.updateGroups(objDevicesViewModel);

            return RedirectToAction("Index");
        }

        public ActionResult SaveGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            objDevicesViewModel.DeviceTypeId = (int)DeviceTypes.OtherDevices;
            otherDevicesBLL.CreateGroups(objDevicesViewModel);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateDevicestoGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            return View();
        }

        public ActionResult DeleteGroup(int Id)
        {
            otherDevicesBLL.removeGroups(Id);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SaveDeviceManager(OtherDeviceGroupViewModel objOtherDeviceGroupViewModel)
        {
            otherDevicesBLL.MapOtherDevicetoGroup(objOtherDeviceGroupViewModel);
            return RedirectToAction("Index");
        }

        public ActionResult DeviceManager(int Id)
        {
            OtherDeviceGroupViewModel objOtherDeviceGroupViewModel = new OtherDeviceGroupViewModel();

            objOtherDeviceGroupViewModel = otherDevicesBLL.GetOtherDeviceGroupDetailsById(Id);

            return View(objOtherDeviceGroupViewModel);
        }

    }
}