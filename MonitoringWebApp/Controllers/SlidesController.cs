using Monitoring.Common;
using Monitoring.Common.CommonModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MonitoringWebService.BAL;
using Monitoring.Common.CommonModels.ViewModels;
using Monitoring.DataAccess.BAL;

namespace MonitoringWebApp.Controllers
{
    public class SlidesController : Controller
    {

        DeviceBLL objDeviceBLL = new DeviceBLL();

        //public ActionResult Index()
        //{
        //    ViewBag.GetSlidesUrl = ConfigurationManager.AppSettings["GetSlidesUrl"].ToString();
        //    ViewBag.GetSlideByIdUrl = ConfigurationManager.AppSettings["GetSlideByIdUrl"].ToString();
        //    return View();
        //}

        public ActionResult Index()
        {
            //ViewBag.GetSlidesUrl = ConfigurationManager.AppSettings["GetSlidesUrl"].ToString();
            //ViewBag.GetSlideByIdUrl = ConfigurationManager.AppSettings["GetSlideByIdUrl"].ToString();
            return View();
        }

        public JsonResult GetSlides()
        {
            MonitoringBLL objMonitoringBLL = new MonitoringBLL();
            var Slides = objMonitoringBLL.GetSlides();
            return Json(Slides, JsonRequestBehavior.AllowGet); ;
        }

        public JsonResult GetSlidesbydeviceType(int Id)
        {
            MonitoringBLL objMonitoringBLL = new MonitoringBLL();
            var Slides = objMonitoringBLL.GetSlidesbydeviceType(Id);
            return Json(Slides, JsonRequestBehavior.AllowGet); ;
        }

        public ActionResult CreateSlides()
        {
            MonitoringBLL objMonitoringBLL = new MonitoringBLL();
            SlidesVideModel objSlidesVideModel = new SlidesVideModel();

            objSlidesVideModel.DeviceTypes = objDeviceBLL.GetDeviceTypes();

            return View(objSlidesVideModel);
        }

        public ActionResult CreateSlidesById(int Id)
        {
            MonitoringBLL objMonitoringBLL = new MonitoringBLL();
            var servers = objMonitoringBLL.GetDeviceBySlide(Id);

            return View(servers);
        }

        public ActionResult EditSlides(int slideId)
        {
            MonitoringBLL objMonitoringBLL = new MonitoringBLL();
            var slide = objMonitoringBLL.GetSlideById(slideId);

            return View(slide);
        }

        public ActionResult SaveSlides(SlidesVideModel ObjSlidesVideModel)
        {
            MonitoringBLL objMonitoringBLL = new MonitoringBLL();
            objMonitoringBLL.AddSlides(ObjSlidesVideModel);
            return Redirect(Url.Action("Index", "Slides") + "?Id=" + ObjSlidesVideModel.DeviceTypeId);
        }

        public ActionResult UpdateSlides(SlidesVideModel ObjSlidesVideModel)
        {
            MonitoringBLL objMonitoringBLL = new MonitoringBLL();
            objMonitoringBLL.UpdateSlides(ObjSlidesVideModel);

            return Redirect(Url.Action("Index", "Slides") + "?Id=" + ObjSlidesVideModel.DeviceTypeId);
            //return RedirectToAction("Index", "Slides", new { Id = ObjSlidesVideModel.DeviceTypeId });
        }

        public ActionResult GetSlideById(int slideId)
        {
            MonitoringBLL objMonitoringBLL = new MonitoringBLL();
            var slide= objMonitoringBLL.GetSlideById(slideId);

            return View(slide);
        }

        public ActionResult DeleteSlides(int slideId)
        {
            MonitoringBLL objMonitoringBLL = new MonitoringBLL();
            objMonitoringBLL.RemoveSlides(slideId);
            return new EmptyResult();
        }


    }
}