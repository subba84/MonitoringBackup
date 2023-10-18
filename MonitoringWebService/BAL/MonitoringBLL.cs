using Monitoring.Common;
using Monitoring.Common.CommonModels;
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
            objtblSlides.Servers = objSlidesVideModel.Servers;
            objtblSlides.SlideName = objSlidesVideModel.SlideName;

            objMonitoringManager.AddSlides(objtblSlides);
        }

        public void UpdateSlides(SlidesVideModel objSlidesVideModel)
        {
            tblSlides objtblSlides = new tblSlides();
            objtblSlides.DeviceTypeId = objSlidesVideModel.DeviceTypeId;
            objtblSlides.IsActive = true;
            objtblSlides.CreatedBy = Constants.CreatedBy;
            objtblSlides.CreatedOn = DateTime.Now;
            objtblSlides.Servers = objSlidesVideModel.Servers;
            objtblSlides.SlideName = objSlidesVideModel.SlideName;
            objtblSlides.SlideId = objSlidesVideModel.SlideId;
            objMonitoringManager.AddSlides(objtblSlides);
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

        public List<ServerGroupViewModel> GetServerGroups()
        {
            List<tblServerGroups> objlsttblServerGroups = new List<tblServerGroups>();
            List<ServerGroupViewModel> objlstGroupViewModel = new List<ServerGroupViewModel>();
            objlsttblServerGroups = objMonitoringManager.GetServerGroups();

            objlsttblServerGroups.ForEach(x => { objlstGroupViewModel.Add(new ServerGroupViewModel() { CpuCritical = x.CpuCritical.HasValue ? x.CpuCritical.Value : 0, CpuWarning = x.CpuWarning.HasValue ? x.CpuWarning.Value : 0, Servers = x.Servers, DiskCritical = x.DiskCritical.HasValue ? x.DiskCritical.Value : 0, DiskWarning = x.DiskWarning.HasValue ? x.DiskWarning.Value : 0, Id = x.Id, MemoryCritical = x.MemoryCritical.HasValue ? x.MemoryCritical.Value : 0, MemoryWarning = x.MemoryWarning.HasValue ? x.MemoryWarning.Value : 0, NetworkCritical = x.NetworkCritical.HasValue ? x.NetworkCritical.Value : 0, NetworkWarning = x.NetworkWarning.HasValue ? x.NetworkWarning.Value : 0 }); });

            return objlstGroupViewModel;
        }

        public List<SlidesVideModel> GetSlides()
        {
            List<tblSlides> objlsttblSlides = new List<tblSlides>();
            List<SlidesVideModel> objlstSlidesVideModel = new List<SlidesVideModel>();

            objlsttblSlides = objMonitoringManager.GetSlides();

            objlsttblSlides.ForEach(x => { objlstSlidesVideModel.Add(new SlidesVideModel() { SlideId = x.SlideId, SlideName = x.SlideName, Servers = x.Servers, DeviceTypeId = x.DeviceTypeId, IsActive = x.IsActive, CreatedBy = x.CreatedBy, CreatedOn = x.CreatedOn }); });

            return objlstSlidesVideModel;
        }

        public List<DashBoardViewModel> GetDashBoards()
        {
            List<DashBoardViewModel> objlstdashBoardViewModels = new List<DashBoardViewModel>();
            List<tblDashBoards> objlsttblDashBoards = new List<tblDashBoards>();

            objlsttblDashBoards = objMonitoringManager.GetDashBoards();
            objlsttblDashBoards.ForEach(x => { objlstdashBoardViewModels.Add(new DashBoardViewModel { DashBoardId = x.DashBoardId, DashBoardName = x.DashBoardName, IsActive = x.IsActive, CreatedOn = x.CreatedOn, CreatedBy = x.CreatedBy, UpdatedBy = x.UpdatedBy, UpdatedOn = x.UpdatedOn }); });
            return objlstdashBoardViewModels;
        }

        public List<DashBoardSlideMappingViewModels> GetDashBoardslideMapping()
        {
            List<DashBoardSlideMappingViewModels> objlstdashBoardSlideMappingViewModels = new List<DashBoardSlideMappingViewModels>();
            List<DashBoardSlideMapping> objlstdashBoardSlideMappings = new List<DashBoardSlideMapping>();
            objlstdashBoardSlideMappings = objMonitoringManager.GetDashBoardSlideMapping();
            objlstdashBoardSlideMappings.ForEach(x => { objlstdashBoardSlideMappingViewModels.Add(new DashBoardSlideMappingViewModels { CreatedBy = x.CreatedBy, CreatedOn = x.CreatedOn, DashBoardId = x.DashBoardId, Id = x.Id, SlideId = x.SlideId, IsActive = x.IsActive }); });
            return objlstdashBoardSlideMappingViewModels;
        }

        public List<AllDashBoardViewModel> GetAllDashBoards()
        {
            List<DashBoardSlideMappingViewModels> objlstdashBoardSlideMappingViewModels = GetDashBoardslideMapping();
            List<DashBoardViewModel> objlstdashBoardViewModels = GetDashBoards();
            List<SlidesVideModel> objlstSlidesVideModel = GetSlides();
            List<AllDashBoardViewModel> objlstAllDashBoardViewModel = new List<AllDashBoardViewModel>();
            foreach (var item in objlstdashBoardSlideMappingViewModels)
            {
                var dashboard = objlstdashBoardViewModels.Where(k => k.DashBoardId == item.DashBoardId).FirstOrDefault();
                var slides = objlstSlidesVideModel.Where(k => k.SlideId == item.SlideId).ToList();

                objlstAllDashBoardViewModel.Add(new AllDashBoardViewModel { DashBoardId = dashboard.DashBoardId, DashBoardName = dashboard.DashBoardName, SlidesVideModel = slides });
            }

            return objlstAllDashBoardViewModel;
        }

        public void CreateDashBoards(AllDashBoardViewModel objAllDashBoardViewModel)
        {
            tblDashBoards objtblDashBoards = new tblDashBoards();

        }

        public void UpdateDashBoards(AllDashBoardViewModel objAllDashBoardViewModel)
        {
            tblDashBoards objtblDashBoards = new tblDashBoards();

        }

        public void RemoveDashBoards(int Id)
        {


        }

        public tblSlides GetSlideById(int slideId)
        {
            return objMonitoringManager.GetSlideById(slideId);
        }

        //public List<string> GetServers()
        //{
        //    return objMonitoringManager.GetServers();
        //}

        public void MapServerstoGroup(tblServerGroups objtblServerGroups)
        {

        }

        public void MapSlidetoDashBoard(DashBoardSlideMappingViewModels objDashBoardSlideMappingViewModels)
        {

        }
    }
}