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
    public class PrinterController : Controller
    {
        PrinterBLL objPrinterBLL = new PrinterBLL();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateGroups()
        {
            return View();
        }

        public JsonResult GetPrinterGroups()
        {
            var result = objPrinterBLL.GetPrinterGroups();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditGroups(int Id)
        {
            var result = objPrinterBLL.GetPrintersGroupsId(Id);
            return View(result);
        }

        public ActionResult UpdateGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            objPrinterBLL.UpdatePrintersGroups(objDevicesViewModel);

            return RedirectToAction("Index");
        }

        public ActionResult SaveGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            objDevicesViewModel.DeviceTypeId = (int)DeviceTypes.Printers;
            objPrinterBLL.CreatePrintersGroups(objDevicesViewModel);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateDevicestoGroups(DeviceGroupViewModel objDevicesViewModel)
        {
            return View();
        }

        public ActionResult DeletePrintersGroups(int Id)
        {
            objPrinterBLL.DeletePrintersGroups(Id);
            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult SaveDeviceManager(PrinterGroupViewModel objPrinterGroupViewModel)
        {
            objPrinterBLL.MapPrinterDevicetoGroup(objPrinterGroupViewModel);
            return RedirectToAction("Index");
        }

        public ActionResult DeviceManager(int Id)
        {
            PrinterGroupViewModel objOtherDeviceGroupViewModel = new PrinterGroupViewModel();

            objOtherDeviceGroupViewModel = objPrinterBLL.GetPrinterGroupDetailsById(Id);

            return View(objOtherDeviceGroupViewModel);
        }

    }
}