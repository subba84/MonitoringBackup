using Monitoring.Common;
using Monitoring.Common.CommonModels;
using Monitoring.Common.CommonModels.ViewModels;
using MonitoringWebService.DAL;
using MonitoringWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MonitoringWebService.BAL
{
    public class MonitoringBLL
    {
        MonitoringManager objMonitoringManager = new MonitoringManager();
        public TimerIntervalsViewModel GetTimerIntervals()
        {
            tblTimerIntervals objtblTimerIntervals = objMonitoringManager.GetTimerIntervals();
            TimerIntervalsViewModel objTimerIntervalsViewModel = new TimerIntervalsViewModel();
            objTimerIntervalsViewModel.CreatedBy = objtblTimerIntervals.CreatedBy;
            objTimerIntervalsViewModel.CreatedOn = objtblTimerIntervals.CreatedOn;
            objTimerIntervalsViewModel.DeviceMonitoringInterval = objtblTimerIntervals.DeviceMonitoringInterval;
            objTimerIntervalsViewModel.HeartBeatInterval = objtblTimerIntervals.HeartBeatInterval;
            objTimerIntervalsViewModel.Id = objtblTimerIntervals.Id;
            objTimerIntervalsViewModel.IsActive = objtblTimerIntervals.IsActive;
            objTimerIntervalsViewModel.MonitoringTimerInterval = objtblTimerIntervals.MonitoringTimerInterval;
            objTimerIntervalsViewModel.NotificationTimerInterval = objtblTimerIntervals.NotificationTimerInterval;
            objTimerIntervalsViewModel.PurgeTimerInterval = objtblTimerIntervals.PurgeTimerInterval;
            objTimerIntervalsViewModel.ReportingTimerInterval = objtblTimerIntervals.ReportingTimerInterval;
            objTimerIntervalsViewModel.MonitoringJsonUploadTimerInterval = objtblTimerIntervals.MonitoringJsonUploadTimerInterval;
            return objTimerIntervalsViewModel;

        }

        public void RegisterDevice(DevicesViewModel objDevicesViewModel)
        {
            tblDevices objtblDevices = new tblDevices();
            objtblDevices.CategoryId = objtblDevices.CategoryId;
            objtblDevices.CreatedBy = objDevicesViewModel.CreatedBy;
            objtblDevices.CreatedOn = objDevicesViewModel.CreatedOn;
            objtblDevices.DeviceName = objDevicesViewModel.DeviceName;
            objtblDevices.DisplayName = objDevicesViewModel.DeviceName;
            objtblDevices.DeviceTypeId = objDevicesViewModel.DeviceTypeId;
            objtblDevices.IpAddress = objDevicesViewModel.IpAddress;
            objtblDevices.UserId = objDevicesViewModel.UserId;
            objtblDevices.Password = objDevicesViewModel.Password;
            objtblDevices.IsActive = objDevicesViewModel.IsActive;
            objtblDevices.Location = objDevicesViewModel.Location;
            objtblDevices.SubCategoryId = objDevicesViewModel.SubCategoryId;
            objtblDevices.VendorId = objDevicesViewModel.VendorId;
            objtblDevices.ManagedTypeId = objDevicesViewModel.ManagedTypeId;

            objMonitoringManager.RegisterDevice(objtblDevices);
        }

        public void UpdateDevice(DevicesViewModel objDevicesViewModel)
        {
            tblDevices objtblDevices = new tblDevices();
            objtblDevices.CategoryId = objtblDevices.CategoryId;
            objtblDevices.CreatedBy = objDevicesViewModel.CreatedBy;
            objtblDevices.CreatedOn = objDevicesViewModel.CreatedOn;
            objtblDevices.DeviceName = objDevicesViewModel.DeviceName;
            objtblDevices.DeviceTypeId = objDevicesViewModel.DeviceTypeId;
            objtblDevices.IpAddress = objDevicesViewModel.IpAddress;
            objtblDevices.UserId = objDevicesViewModel.UserId;
            objtblDevices.Password = objDevicesViewModel.Password;
            objtblDevices.IsActive = objDevicesViewModel.IsActive;
            objtblDevices.Location = objDevicesViewModel.Location;
            objtblDevices.SubCategoryId = objDevicesViewModel.SubCategoryId;
            objtblDevices.VendorId = objDevicesViewModel.VendorId;
            objtblDevices.ManagedTypeId = objDevicesViewModel.ManagedTypeId;
            objMonitoringManager.UpdateDevice(objtblDevices);
        }

        public List<DevicesViewModel> GetDeviceByTypeId(int deviceTypeId)
        {
            List<DevicesViewModel> objLstDevicesViewModels = new List<DevicesViewModel>();

            var deviceData = objMonitoringManager.GetDeviceByTypeId(deviceTypeId);

            deviceData.ForEach(x => { objLstDevicesViewModels.Add(new DevicesViewModel() { DeviceId = x.DeviceId, DeviceTypeId = x.DeviceTypeId, CategoryId = x.CategoryId, CreatedOn = x.CreatedOn, CreatedBy = x.CreatedBy, DeviceName = x.DeviceName }); });

            return objLstDevicesViewModels;
        }

        public void UpdateMonirotingDetails(List<ServerMonitoringModel> ObjmonitoringModel)
        {
            objMonitoringManager.UpdateMonirotingDetails(ObjmonitoringModel);
        }

        public void AddSlides(SlidesVideModel objSlidesVideModel)
        {
            tblSlides objtblSlides = new tblSlides();
            objtblSlides.DeviceTypeId = objSlidesVideModel.DeviceTypeId;
            objtblSlides.IsActive = true;
            objtblSlides.CreatedBy = Constants.CreatedBy;
            objtblSlides.CreatedOn = DateTime.Now;
            objtblSlides.Devices = string.Join(",", objSlidesVideModel.SelectedDevices);
            objtblSlides.SlideName = objSlidesVideModel.SlideName;

            objMonitoringManager.AddSlides(objtblSlides);
        }

        public void UpdateSlides(SlidesVideModel objSlidesVideModel)
        {
            tblSlides objtblSlides = new tblSlides();
            objtblSlides.SlideId = objSlidesVideModel.SlideId;
            objtblSlides.Devices = string.Join(",", objSlidesVideModel.SelectedDevices);
            objtblSlides.SlideName = objSlidesVideModel.SlideName;
            objMonitoringManager.UpdateSlides(objtblSlides);
        }

        public void RemoveSlides(int Id)
        {
            objMonitoringManager.RemoveSlides(Id);
        }

        public void CreateGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            objMonitoringManager.CreateGroups(objDeviceGroupViewModel);
        }

        public void updateGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            objMonitoringManager.UpdateGroups(objDeviceGroupViewModel);
        }

        public void removeGroups(int Id)
        {
            objMonitoringManager.RemoveGroups(Id);
        }

        public List<GroupViewModel> GetGroups()
        {
            List<tblDeviceGroups> objlsttblServerGroups = new List<tblDeviceGroups>();
            List<GroupViewModel> objlstGroupViewModel = new List<GroupViewModel>();
            objlsttblServerGroups = objMonitoringManager.GetGroups();

            objlsttblServerGroups.ForEach(x => { objlstGroupViewModel.Add(new GroupViewModel() { GroupId = x.GroupId, DeviceTypeName = ((DeviceTypes)x.DeviceTypeId).ToString(), GroupName = x.GroupName }); });

            return objlstGroupViewModel;
        }

        public DeviceGroupViewModel GetGroupsbyId(int Id)
        {
            return objMonitoringManager.GetGroupsbyId(Id);
        }

        public ServerGroupViewModel GetServerGroupById(int Id)
        {
            ServerGroupViewModel objServerGroupViewModel = new ServerGroupViewModel();
            tblServerGroups objtblServerGroups = new tblServerGroups();
            List<tblDevices> objServers = new List<tblDevices>();
            List<DevicesViewModel> objDeviceViewModel = new List<DevicesViewModel>();
            DeviceGroupViewModel objDeviceGroupViewModel = new DeviceGroupViewModel();

            objtblServerGroups = objMonitoringManager.GetServerGroupById(Id);

            objDeviceGroupViewModel = objMonitoringManager.GetGroupsbyId(Id);

            objServerGroupViewModel.GroupId = objtblServerGroups.GroupId;

            objServerGroupViewModel.GroupName = objDeviceGroupViewModel.GroupName;

            if (!string.IsNullOrEmpty(objtblServerGroups.Servers))
            {
                objServerGroupViewModel.SelectedServers = objtblServerGroups.Servers.Split(',').ToList();
            }

            objServers = objMonitoringManager.GetDeviceByTypeId((int)DeviceTypes.Servers);

            objServers.ForEach(x => { objDeviceViewModel.Add(new DevicesViewModel() { DeviceId = x.DeviceId, DeviceName = x.DeviceName }); });

            objServerGroupViewModel.Servers = objDeviceViewModel;

            return objServerGroupViewModel;
        }

        public SlidesVideModel GetServerBySlide()
        {

            List<tblDevices> objServers = new List<tblDevices>();
            List<DevicesViewModel> objDeviceViewModel = new List<DevicesViewModel>();
            SlidesVideModel objSlidesVideModel = new SlidesVideModel();

            objServers = objMonitoringManager.GetDeviceByTypeId((int)DeviceTypes.Servers);

            objServers.ForEach(x => { objDeviceViewModel.Add(new DevicesViewModel() { DeviceId = x.DeviceId, DeviceName = x.DeviceName }); });

            objSlidesVideModel.Devices = objDeviceViewModel;

            return objSlidesVideModel;
        }

        public SlidesVideModel GetDeviceBySlide(int Id)
        {

            List<tblDevices> objDevices = new List<tblDevices>();
            List<DevicesViewModel> objDeviceViewModel = new List<DevicesViewModel>();
            SlidesVideModel objSlidesVideModel = new SlidesVideModel();

            objDevices = objMonitoringManager.GetDeviceByTypeId(Id);

            objDevices.ForEach(x => { objDeviceViewModel.Add(new DevicesViewModel() { DeviceId = x.DeviceId, DeviceName = x.DeviceName }); });

            objSlidesVideModel.Devices = objDeviceViewModel;

            return objSlidesVideModel;
        }

        public List<SlidesVideModel> GetSlides()
        {
            List<tblSlides> objlsttblSlides = new List<tblSlides>();
            List<SlidesVideModel> objlstSlidesVideModel = new List<SlidesVideModel>();

            objlsttblSlides = objMonitoringManager.GetSlides();

            objlsttblSlides.ForEach(x => { objlstSlidesVideModel.Add(new SlidesVideModel() { SlideId = x.SlideId, SlideName = x.SlideName, SelectedDevices = x.Devices.Split(',').ToList(), DeviceTypeId = x.DeviceTypeId, DeviceTypeName = DeviceTypes.Servers.ToString(), IsActive = x.IsActive, CreatedBy = x.CreatedBy, CreatedOn = x.CreatedOn }); });

            return objlstSlidesVideModel;
        }

        public List<SlidesVideModel> GetSlidesbydeviceType(int Id)
        {
            List<tblSlides> objlsttblSlides = new List<tblSlides>();
            List<SlidesVideModel> objlstSlidesVideModel = new List<SlidesVideModel>();

            objlsttblSlides = objMonitoringManager.GetSlidesbydeviceType(Id);

            objlsttblSlides.ForEach(x => { objlstSlidesVideModel.Add(new SlidesVideModel() { SlideId = x.SlideId, SlideName = x.SlideName, SelectedDevices = x.Devices.Split(',').ToList(), DeviceTypeId = x.DeviceTypeId, DeviceTypeName = DeviceTypes.Servers.ToString(), IsActive = x.IsActive, CreatedBy = x.CreatedBy, CreatedOn = x.CreatedOn }); });

            return objlstSlidesVideModel;
        }

        public List<SlidesVideModel> GetSlideswithServerName()
        {
            List<tblSlides> objlsttblSlides = new List<tblSlides>();
            List<SlidesVideModel> objlstSlidesVideModel = new List<SlidesVideModel>();

            objlsttblSlides = objMonitoringManager.GetSlides();

            objlsttblSlides.ForEach(x => { objlstSlidesVideModel.Add(new SlidesVideModel() { SlideId = x.SlideId, SlideName = x.SlideName, DeviceTypeId = x.DeviceTypeId, DeviceTypeName = DeviceTypes.Servers.ToString(), IsActive = x.IsActive, CreatedBy = x.CreatedBy, CreatedOn = x.CreatedOn }); });

            return objlstSlidesVideModel;
        }

        public List<AllDashBoardViewModel> GetDashBoards()
        {
            List<AllDashBoardViewModel> objlstdashBoardViewModels = new List<AllDashBoardViewModel>();
            List<tblDashBoards> objlsttblDashBoards = new List<tblDashBoards>();

            objlsttblDashBoards = objMonitoringManager.GetDashBoards();
            objlsttblDashBoards.ForEach(x => { objlstdashBoardViewModels.Add(new AllDashBoardViewModel { DashBoardId = x.DashBoardId, DashBoardName = x.DashBoardName, IsActive = x.IsActive, CreatedOn = x.CreatedOn, CreatedBy = x.CreatedBy }); });
            return objlstdashBoardViewModels;
        }

        public AllDashBoardViewModel GetAllDashBoardsById(int dashBoardId)
        {
            AllDashBoardViewModel allDashBoardViewModel = new AllDashBoardViewModel();
            DashBoardViewModel dashBoardViewModel = new DashBoardViewModel();
            tblDashBoards objTblDashBoards = new tblDashBoards();
            DashBoardSlideMapping dashBoardSlideMapping = new DashBoardSlideMapping();

            dashBoardSlideMapping = objMonitoringManager.GetDashBoardSlideMapping(dashBoardId);
            objTblDashBoards = objMonitoringManager.GetDashBoardById(dashBoardId);

            allDashBoardViewModel.DashBoardId = dashBoardId;
            allDashBoardViewModel.DashBoardName = objTblDashBoards.DashBoardName;
            if (!string.IsNullOrEmpty(dashBoardSlideMapping.Slides))
            {
                allDashBoardViewModel.SelectedSlides = dashBoardSlideMapping.Slides.Split(',').Select(Int32.Parse).ToList();
            }
            allDashBoardViewModel.Slides = GetSlides();

            return allDashBoardViewModel;


        }

        public void SaveDashBoards(AllDashBoardViewModel objAllDashBoardViewModel)
        {
            objMonitoringManager.CreateDashBoards(objAllDashBoardViewModel);
        }

        public void UpdateDashBoards(AllDashBoardViewModel objAllDashBoardViewModel)
        {
            objMonitoringManager.UpdateDashBoards(objAllDashBoardViewModel);
        }

        public void RemoveDashBoards(int dashBoardId)
        {
            objMonitoringManager.removeDashBoards(dashBoardId);
        }

        public SlidesVideModel GetSlideById(int slideId)
        {
            SlidesVideModel slidesVideModel = new SlidesVideModel();
            List<tblDevices> objDevices = new List<tblDevices>();
            List<DevicesViewModel> objDeviceViewModel = new List<DevicesViewModel>();

            var slide = objMonitoringManager.GetSlideById(slideId);

            slidesVideModel.SlideId = slide.SlideId;
            slidesVideModel.SlideName = slide.SlideName;
            slidesVideModel.DeviceTypeId = slide.DeviceTypeId;
            slidesVideModel.DeviceTypeName = DeviceTypes.Servers.ToString();

            if (!string.IsNullOrEmpty(slide.Devices))
            {
                slidesVideModel.SelectedDevices = slide.Devices.Split(',').ToList();
            }

            objDevices = objMonitoringManager.GetDeviceByTypeId(slide.DeviceTypeId);

            objDevices.ForEach(x => { objDeviceViewModel.Add(new DevicesViewModel() { DeviceId = x.DeviceId, DeviceName = x.DeviceName }); });

            slidesVideModel.Devices = objDeviceViewModel;

            return slidesVideModel;
        }

        //public List<string> GetServers()
        //{
        //    return objMonitoringManager.GetServers();
        //}

        public void MapServerstoGroup(ServerGroupViewModel objServergroupViewModel)
        {
            tblServerGroups objtblServerGroups = new tblServerGroups();
            objtblServerGroups.GroupId = objServergroupViewModel.GroupId;
            objtblServerGroups.Servers = string.Join(",", objServergroupViewModel.SelectedServers);

            objMonitoringManager.MapServerstoGroup(objtblServerGroups);
        }

        public void MapSlidetoDashBoard(DashBoardSlideMappingViewModels objDashBoardSlideMappingViewModels)
        {

        }

        public List<ServerGroupViewModel> GetServerGroups()
        {
            List<ServerGroupViewModel> serverGroupViewModels = new List<ServerGroupViewModel>();

            List<tblServerGroups> tblServerGroups = new List<tblServerGroups>();
            tblServerGroups = objMonitoringManager.GetServerGroups();


            tblServerGroups.ForEach(x => { serverGroupViewModels.Add(new ServerGroupViewModel() { Id = x.Id, SelectedServers = x.Servers.Split(',').ToList(), CpuCritical = x.CpuCritical.HasValue ? x.CpuCritical.Value : 0, CpuWarning = x.CpuWarning.HasValue ? x.CpuWarning.Value : 0, DiskCritical = x.DiskCritical.HasValue ? x.DiskCritical.Value : 0, DiskWarning = x.DiskWarning.HasValue ? x.DiskCritical.Value : 0, GroupId = x.GroupId, MemoryCritical = x.MemoryCritical.HasValue ? x.MemoryCritical.Value : 0, MemoryWarning = x.MemoryWarning.HasValue ? x.MemoryWarning.Value : 0, NetworkCritical = x.NetworkCritical.HasValue ? x.NetworkCritical.Value : 0, NetworkWarning = x.NetworkWarning.HasValue ? x.NetworkWarning.Value : 0 }); });

            return serverGroupViewModels;

        }

        public AppSettingsVideModel GetAppSettings()
        {
            AppSettingsVideModel objAppSettingsVideModel = new AppSettingsVideModel();

            tblAppSettings objTblAppSettings = new tblAppSettings();

            objTblAppSettings = objMonitoringManager.GetAppSettings();

            objAppSettingsVideModel.MonitoringLogLocation = objTblAppSettings.MonitoringLogLocation;

            objAppSettingsVideModel.MonitoringCommonLogLocation = objTblAppSettings.MonitoringCommonLogLocation;

            objAppSettingsVideModel.PingDevicesLogLocation = objTblAppSettings.PingDevicesLogLocation;

            return objAppSettingsVideModel;
        }

        public List<DevicesViewModel> GetPingDevices()
        {
            List<DevicesViewModel> objlstDevicesViewModel = new List<DevicesViewModel>();

            List<tblDevices> tblPingDevices = new List<tblDevices>();
            tblPingDevices = objMonitoringManager.GetPingDevices();

            tblPingDevices.ForEach(x => { objlstDevicesViewModel.Add(new DevicesViewModel() { DeviceId = x.DeviceId, DeviceName = x.DeviceName }); });

            return objlstDevicesViewModel;

        }

        public void UpdateDeviceStatus(int deviceId, int status)
        {
            objMonitoringManager.UpdateDeviceStatus(deviceId, status);
        }

        public List<PingDeviceDetails> GetOtherDevicesDetails()
        {
            return objMonitoringManager.GetOtherDevicesDetails();
        }
    }
}