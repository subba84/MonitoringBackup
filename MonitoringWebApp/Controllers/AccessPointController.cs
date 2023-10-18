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
    public class AccessPointController : Controller
    {
        AccessPointsBLL objAccessPointsBLL = new AccessPointsBLL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateGroups()
        {
            return View();
        }

        public JsonResult GetAccessPointGroups()
        {
            var result = objAccessPointsBLL.GetAccessPointGroups();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditGroups(int Id)
        {
            var result = objAccessPointsBLL.GetAccessPointGroupsId(Id);
            return View(result);
        }

        public ActionResult UpdateGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            objAccessPointsBLL.UpdateAccessPointGroups(objDevicesViewModel);

            return RedirectToAction("Index");
        }

        public ActionResult SaveGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            objDevicesViewModel.DeviceTypeId = (int)DeviceTypes.AccessPoints;
            objAccessPointsBLL.CreateAccessPointGroups(objDevicesViewModel);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateDevicestoGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            return View();
        }

        public ActionResult DeleteAccessPointGroups(int Id)
        {
            objAccessPointsBLL.DeleteAccessPointGroups(Id);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SaveDeviceManager(AccessPointGroupViewModel objAccessPointGroupViewModel)
        {
            objAccessPointsBLL.MapAccessPointstoGroup(objAccessPointGroupViewModel);
            return RedirectToAction("Index");
        }

        public ActionResult DeviceManager(int Id)
        {
            AccessPointGroupViewModel objAccessPointGroupViewModel = new AccessPointGroupViewModel();

            objAccessPointGroupViewModel = objAccessPointsBLL.GetAccessPointGroupDetailsById(Id);

            return View(objAccessPointGroupViewModel);
        }

    }
}