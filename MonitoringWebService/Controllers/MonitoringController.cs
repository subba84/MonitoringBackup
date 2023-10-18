using Monitoring.Common;
using Monitoring.Common.CommonModels;
using Monitoring.Common.CommonModels.ViewModels;
using Monitoring.DataAccess.BAL;
using MonitoringWebService.BAL;
using MonitoringWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Caching;
using System.Web;
using System.Web.Http;

namespace MonitoringWebService.Controllers
{
    public class MonitoringController : ApiController
    {
        MonitoringBLL objMonitoringBLL = new MonitoringBLL();
        DeviceBLL objDeviceBLL = new DeviceBLL();

        [Route("UpdateServerHeartBeat")]
        [HttpPost]
        public void UpdateServerHeartBeat(ServerMonitoringModel objHeartBeatModel)
        {
            MemoryCache cache = MemoryCache.Default;
            List<ServerMonitoringModel> objLstHeartBeatModel = new List<ServerMonitoringModel>();

            if (cache.Get("ServerHeartBeat") != null)
            {
                objLstHeartBeatModel = (List<ServerMonitoringModel>)cache.Get("ServerHeartBeat");
                var existingRecord = objLstHeartBeatModel.Where(k => k.ServerName == objHeartBeatModel.ServerName).FirstOrDefault();
                if (existingRecord != null)
                {
                    objLstHeartBeatModel.Remove(existingRecord);
                }
            }

            objLstHeartBeatModel.Add(objHeartBeatModel);

            var cachePolicty = new CacheItemPolicy();
            cachePolicty.AbsoluteExpiration = DateTime.Now.AddYears(1);

            cache.Add("ServerHeartBeat", objLstHeartBeatModel, cachePolicty); ;
        }

        [Route("GetServerHeartBeat")]
        [HttpGet]
        public List<ServerMonitoringModel> GetServerHeartBeat()
        {
            List<ServerMonitoringModel> objLstHeartBeatModel = new List<ServerMonitoringModel>();
            MemoryCache cache = MemoryCache.Default;
            if (cache.Get("ServerHeartBeat") != null)
            {
                objLstHeartBeatModel = (List<ServerMonitoringModel>)cache.Get("ServerHeartBeat");
            }

            //var result = objLstHeartBeatModel.Where(k => (DateTime.Now - k.LastDiscoveredTime).Value.Minutes <= 1).ToList();

            return objLstHeartBeatModel;
        }

        [Route("GetTimerIntervals")]
        [HttpGet]
        public TimerIntervalsViewModel GetTimerIntervals()
        {
            return objMonitoringBLL.GetTimerIntervals();
        }

        [Route("UpdateMonirotingDetails")]
        [HttpPost]
        public void UpdateMonirotingDetails(List<ServerMonitoringModel> ObjmonitoringModel)
        {
            objMonitoringBLL.UpdateMonirotingDetails(ObjmonitoringModel);
        }

        [Route("UpdateJsonMonirotingDetails")]
        [HttpPost]
        public void UpdateJsonMonirotingDetails(List<ServerMonitoringModel> ObjmonitoringModel)
        {
            objMonitoringBLL.UpdateMonirotingDetails(ObjmonitoringModel);
        }

        [Route("RegisterDevice")]
        [HttpPost]
        public void RegisterDevice(DevicesViewModel objDevicesViewModels)
        {
            objMonitoringBLL.RegisterDevice(objDevicesViewModels);
        }

        [Route("GetDashBoard")]
        [HttpPost]
        public List<HomeViewModel> GetDashBoard()
        {
            return null;
        }

        [Route("GetDashBoardData")]
        [HttpGet]
        public HomeViewModel GetDashBoardData(int SlideId)
        {
            List<ServerMonitoringModel> objlstServerMonitoringModel = new List<ServerMonitoringModel>();
            objlstServerMonitoringModel = GetServerHeartBeat();

            List<ServerGroupViewModel> lstgroupViewModels = new List<ServerGroupViewModel>();
            lstgroupViewModels = objMonitoringBLL.GetServerGroups();

            List<SlidesVideModel> lstslidesVideModels = new List<SlidesVideModel>();
            lstslidesVideModels = GetSlides();

            HomeViewModel lsthomeViewModels = new HomeViewModel();
            List<SlideData> objlstSlideData = new List<SlideData>();

            List<ServerAnalytics> objlstserverAnalytics = new List<ServerAnalytics>();

            ServerMonitoringModel serverMonitoringModel = new ServerMonitoringModel();
            ServerGroupViewModel thresholds = new ServerGroupViewModel();

            foreach (var item in lstslidesVideModels.Where(k => k.SlideId == SlideId))
            {
                List<string> slideDevices = item.SelectedDevices;
                if (item.DeviceTypeId == (int)DeviceTypes.Servers)
                {
                    objlstserverAnalytics = new List<ServerAnalytics>();

                    foreach (var server in slideDevices)
                    {
                        thresholds = new ServerGroupViewModel();

                        serverMonitoringModel = new ServerMonitoringModel();

                        ServerAnalytics serverAnalytics = new ServerAnalytics();

                        serverAnalytics.ServerDisplayName = server;

                        thresholds = lstgroupViewModels.Where(k => k.SelectedServers.Contains(server)).FirstOrDefault();

                        serverMonitoringModel = objlstServerMonitoringModel.Where(k => k.ServerName == server).FirstOrDefault();

                        if (thresholds != null && serverMonitoringModel != null)
                        {
                            serverAnalytics.CpuUtilization = serverMonitoringModel.CpuDetails.CpuUtilization;
                            serverAnalytics.RamUtilization = serverMonitoringModel.MemoryDetails.MemoryUtilization;

                            if (serverMonitoringModel.CpuDetails.CpuUtilization > thresholds.CpuCritical || serverMonitoringModel.MemoryDetails.MemoryUtilization > thresholds.MemoryCritical)
                            {
                                serverAnalytics.Colour = "Red";
                            }
                            else if ((serverMonitoringModel.CpuDetails.CpuUtilization > thresholds.CpuWarning && serverMonitoringModel.CpuDetails.CpuUtilization < thresholds.CpuCritical) || (serverMonitoringModel.MemoryDetails.MemoryUtilization > thresholds.MemoryWarning && serverMonitoringModel.MemoryDetails.MemoryUtilization < thresholds.MemoryCritical))
                            {
                                serverAnalytics.Colour = "Yellow";
                            }
                            else
                            {
                                serverAnalytics.Colour = "Green";
                            }

                            objlstserverAnalytics.Add(serverAnalytics);
                        }
                    }

                    objlstSlideData.Add(new SlideData() { ServerAnalytics = objlstserverAnalytics, SlideId = item.SlideId, SlideName = item.SlideName, DeviceTypeId = item.DeviceTypeId });
                }

                else
                {
                    List<OtherDeviceAnalytics> objlstOtherDeviceAnalytics = new List<OtherDeviceAnalytics>();
                    
                    foreach (var device in slideDevices)
                    {
                        OtherDeviceAnalytics objOtherDeviceAnalytics = new OtherDeviceAnalytics();

                        var deviceDetails = objDeviceBLL.GetDeviceByName(device);
                        objOtherDeviceAnalytics.OtherDeviceName = deviceDetails.DeviceName;

                        if (deviceDetails.Status == (int)DeviceStatus.Working)
                        {
                            objOtherDeviceAnalytics.Colour = "Green";
                        }
                        else
                        {
                            objOtherDeviceAnalytics.Colour = "Red";
                        }

                        objlstOtherDeviceAnalytics.Add(objOtherDeviceAnalytics);
                    }

                    objlstSlideData.Add(new SlideData() { OtherDeviceAnalytics = objlstOtherDeviceAnalytics, SlideId = item.SlideId, SlideName = item.SlideName, DeviceTypeId = item.DeviceTypeId });
                }
            }

            lsthomeViewModels.SlideData = objlstSlideData;

            return lsthomeViewModels;
        }

        [Route("GetSlides")]
        [HttpGet]
        public List<SlidesVideModel> GetSlides()
        {
            return objMonitoringBLL.GetSlides();
        }

        //[Route("GetSlideById")]
        //[HttpGet]
        //public tblSlides GetSlideById(int slideId)
        //{
        //    return objMonitoringBLL.GetSlideById(slideId);
        //}

        [Route("GetHeartBeatByServer")]
        [HttpGet]
        public List<ServerMonitoringModel> GetHeartBeatByServer(string serverName)
        {
            List<ServerMonitoringModel> objLstHeartBeatModel = new List<ServerMonitoringModel>();
            MemoryCache cache = MemoryCache.Default;
            if (cache.Get("ServerHeartBeat") != null)
            {
                objLstHeartBeatModel = (List<ServerMonitoringModel>)cache.Get("ServerHeartBeat");
            }

            if (objLstHeartBeatModel != null && objLstHeartBeatModel.Count > 0)
            {
                return objLstHeartBeatModel.Where(k => k.ServerName == serverName).ToList();
            }

            return null;

            //var result = objLstHeartBeatModel.Where(k => (DateTime.Now - k.LastDiscoveredTime).Value.Minutes <= 1).ToList();
        }

        [Route("GetDeviceByTypeId")]
        [HttpGet]
        public List<DevicesViewModel> GetDeviceByTypeId(int deviceTypeId)
        {
            return objMonitoringBLL.GetDeviceByTypeId(deviceTypeId);
        }

        [Route("UpdateDevice")]
        [HttpPost]
        public void UpdateDevice(DevicesViewModel objDevicesViewModel)
        {
            objMonitoringBLL.UpdateDevice(objDevicesViewModel);
        }

        [Route("GetAppSettings")]
        [HttpGet]
        public AppSettingsVideModel GetAppSettings()
        {
            return objMonitoringBLL.GetAppSettings();
        }

        [Route("GetPingDevices")]
        [HttpGet]
        public List<DevicesViewModel> GetPingDevices()
        {
            return objMonitoringBLL.GetPingDevices();
        }

        [Route("UpdateDeviceStatus")]
        [HttpGet]
        public void UpdateDeviceStatus(int deviceId, int status)
        {
            objMonitoringBLL.UpdateDeviceStatus(deviceId, status);
        }

        [Route("GetOtherDevicesDetails")]
        [HttpGet]
        public List<PingDeviceDetails> GetOtherDevicesDetails()
        {
            return objMonitoringBLL.GetOtherDevicesDetails();
        }

        [Route("GetAccessPointDetails")]
        [HttpGet]
        public List<PingDeviceDetails> GetAccessPointDetails()
        {
            AccessPointsBLL objAccessPointsBLL = new AccessPointsBLL();
            return objAccessPointsBLL.GetAccessPointDetails();
        }
    }
}
