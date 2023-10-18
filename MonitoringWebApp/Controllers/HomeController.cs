using Monitoring.Common;
using Monitoring.Common.CommonModels;
using Monitoring.Common.CommonModels.ViewModels;
using MonitoringWebService.BAL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MonitoringWebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.GetSlidesUrl = ConfigurationManager.AppSettings["GetSlidesUrl"].ToString();
            ViewBag.GetSlideByIdUrl = ConfigurationManager.AppSettings["GetSlideByIdUrl"].ToString();
            return View();
        }
    }
}