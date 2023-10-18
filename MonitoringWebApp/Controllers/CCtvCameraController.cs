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
    public class CCtvCameraController : Controller
    {
        CctvCameraBLL objCctvCameraBLL = new CctvCameraBLL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateGroups()
        {
            return View();
        }

        public JsonResult GetCCTVCameraGroups()
        {
            var result = objCctvCameraBLL.GetCCTVCameraGroups();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditGroups(int Id)
        {
            var result = objCctvCameraBLL.GetCctvCameraGroupsId(Id);
            return View(result);
        }

        public ActionResult UpdateGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            objCctvCameraBLL.UpdateCctvCameraGroups(objDevicesViewModel);

            return RedirectToAction("Index");
        }

        public ActionResult SaveGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            objDevicesViewModel.DeviceTypeId = (int)DeviceTypes.CCTVCameras;
            objCctvCameraBLL.CreateCctvCameraGroups(objDevicesViewModel);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateDevicestoGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            return View();
        }

        public ActionResult DeleteCctvCameraGroups(int Id)
        {
            objCctvCameraBLL.DeleteCctvCameraGroups(Id);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SaveDeviceManager(CctvCameraGroupViewModel objCctvCameraGroupViewModel)
        {
            objCctvCameraBLL.MapCctvCamerastoGroup(objCctvCameraGroupViewModel);
            return RedirectToAction("Index");
        }

        public ActionResult DeviceManager(int Id)
        {
            CctvCameraGroupViewModel objCctvCameraGroupViewModel = new CctvCameraGroupViewModel();

            objCctvCameraGroupViewModel = objCctvCameraBLL.GetCCtvCameraGroupDetailsById(Id);

            return View(objCctvCameraGroupViewModel);
        }

    }
}