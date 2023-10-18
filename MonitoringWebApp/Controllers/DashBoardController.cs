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

namespace MonitoringWebApp.Controllers
{
    public class DashBoardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetDashBoards()
        {
            MonitoringBLL monitoringBLL = new MonitoringBLL();
            var dashboards = monitoringBLL.GetDashBoards();
            return Json(dashboards, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateDashBoard()
        {
            MonitoringBLL monitoringBLL = new MonitoringBLL();
            AllDashBoardViewModel allDashBoardViewModel = new AllDashBoardViewModel();
            allDashBoardViewModel.Slides = monitoringBLL.GetSlides();

            return View(allDashBoardViewModel);
        }

        public ActionResult SaveDashBoard(AllDashBoardViewModel allDashBoardViewModel)
        {
            MonitoringBLL monitoringBLL = new MonitoringBLL();
            monitoringBLL.SaveDashBoards(allDashBoardViewModel);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateDashBoard(AllDashBoardViewModel allDashBoardViewModel)
        {
            MonitoringBLL monitoringBLL = new MonitoringBLL();
            monitoringBLL.UpdateDashBoards(allDashBoardViewModel);
            return RedirectToAction("Index");
        }

        public ActionResult DeleteDashBoard(int dashBoardId)
        {
            MonitoringBLL monitoringBLL = new MonitoringBLL();
            monitoringBLL.RemoveDashBoards(dashBoardId);
            return new EmptyResult();
        }

        public ActionResult EditDashBoard(int dashBoardId)
        {
            MonitoringBLL monitoringBLL = new MonitoringBLL();
            var result = monitoringBLL.GetAllDashBoardsById(dashBoardId);
            return View(result);
        }

    }
}