using Monitoring.Common;
using Monitoring.Common.CommonModels;
using Monitoring.Common.CommonModels.ViewModels;
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
    public class DeviceController : Controller
    {
        DeviceBLL objDeviceBLL = new DeviceBLL();

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult Index(int Id)
        {
            ViewBag.deviceTypeId = Id;
            return View();
        }

        public JsonResult GetDeviceByTypeById(int Id)
        {
            var devices = objDeviceBLL.GetDeviceByTypeById(Id);
            return Json(devices, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateDevice(int Id)
        {
            DevicesViewModel objDevicesViewModel = new DevicesViewModel();
            objDevicesViewModel.DeviceTypes = objDeviceBLL.GetDeviceTypes();
            objDevicesViewModel.DeviceTypeId = Id;
            return View(objDevicesViewModel);
        }

        [HttpPost]
        public ActionResult SaveDevice(DevicesViewModel objDevicesViewModel)
        {
            objDeviceBLL.AddDevice(objDevicesViewModel);
            return Redirect(Url.Action("Index", "Device") + "?Id=" + objDevicesViewModel.DeviceTypeId);
        }

        [HttpPost]
        public ActionResult UpdateDevice(DevicesViewModel objDevicesViewModel)
        {
            objDeviceBLL.UpdateDevice(objDevicesViewModel);
            return Redirect(Url.Action("Index", "Device") + "?Id=" + objDevicesViewModel.DeviceTypeId);
        }

        public ActionResult DeleteDevice(int Id)
        {
            objDeviceBLL.DeleteDevice(Id);
            return new EmptyResult();
        }

        public ActionResult EditDevice(int Id)
        {
            var device = objDeviceBLL.GetDeviceById(Id);
            device.DeviceTypes = objDeviceBLL.GetDeviceTypes();
            return View(device);
        }
    }
}