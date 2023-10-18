using Monitoring.Common;
using MonitoringWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Monitoring.Common.Logging;
using Monitoring.Common.CommonModels;

namespace MonitoringWebService.DAL
{
    public class MonitoringManager
    {
        public tblTimerIntervals GetTimerIntervals()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblTimerIntervals.Where(k => k.IsActive == true).FirstOrDefault();
            }
        }

        public void RegisterDevice(tblDevices objtblDevices)
        {
            using (var context = new MonitoringContext())
            {
                if (!context.tblDevices.Where(k => k.DeviceName == objtblDevices.DeviceName && k.IsActive == true).Any())
                {
                    context.tblDevices.Add(objtblDevices);
                    context.SaveChanges();
                }
            }
        }

        public void UpdateDevice(tblDevices objtblDevices)
        {
            using (var context = new MonitoringContext())
            {
                var existingrecord = context.tblDevices.Where(k => k.DeviceId == objtblDevices.DeviceId && k.IsActive == true).FirstOrDefault();
                if (existingrecord!=null)
                {
                    existingrecord = objtblDevices;
                    context.SaveChanges();
                }
            }
        }

        public List<tblDevices> GetDeviceByTypeId(int deviceTypeId)
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDevices.Where(k => k.DeviceTypeId == deviceTypeId && k.IsActive == true).ToList();
            }
        }

        public tblAppSettings GetAppSettings()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblAppSettings.Where(k => k.IsActive == true).FirstOrDefault();
            }
        }

        public void UpdateMonirotingDetails(List<ServerMonitoringModel> ObjmonitoringModel)
        {
            var appSettings = GetAppSettings();

            DetailsLogger objdetailsLogger = new DetailsLogger();
            objdetailsLogger.UpdateServerMonirotingDetails(ObjmonitoringModel, appSettings.MonitoringLogLocation);
        }

        public void UpdateCommonMonirotingDetails(List<ServerMonitoringModel> ObjmonitoringModel)
        {
            var appSettings = GetAppSettings();

            DetailsLogger objdetailsLogger = new DetailsLogger();
            objdetailsLogger.UpdateServerMonirotingDetails(ObjmonitoringModel, appSettings.MonitoringCommonLogLocation);
        }

        public void AddSlides(tblSlides objtblSlides)
        {
            using (var context = new MonitoringContext())
            {
                if (!context.tblSlides.Where(k => k.SlideName == objtblSlides.SlideName && k.IsActive == true).Any())
                {
                    context.tblSlides.Add(objtblSlides);
                    context.SaveChanges();
                }
            }
        }

        public List<tblSlides> GetSlides()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblSlides.Where(k => k.IsActive == true).ToList();
            }
        }

        public void UpdateSlides(tblSlides objtblSlides)
        {
            using (var context = new MonitoringContext())
            {
                var existingRecord = context.tblSlides.Where(k => k.SlideName == objtblSlides.SlideName && k.IsActive == true).FirstOrDefault();
                if (existingRecord != null)
                {
                    existingRecord = objtblSlides;
                    context.SaveChanges();
                }
            }
        }

        public void RemoveSlides(int Id)
        {
            using (var context = new MonitoringContext())
            {
                var existingRecord = context.tblSlides.Where(k => k.SlideId == Id).FirstOrDefault();
                if (existingRecord != null)
                {
                    existingRecord.IsActive = false;
                    context.SaveChanges();
                }
            }
        }

        public void CreateGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            using (var context = new MonitoringContext())
            {
                tblDeviceGroups objtblDeviceGroups = new tblDeviceGroups() { GroupName = objDeviceGroupViewModel.GroupName, DeviceTypeId = objDeviceGroupViewModel.DeviceTypeId, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.Now };
                context.tblDeviceGroups.Add(objtblDeviceGroups);
                context.SaveChanges();

                if (objDeviceGroupViewModel.DeviceTypeId == (int)DeviceTypes.Servers)
                {

                    tblServerGroups objtblServerGroups = new tblServerGroups();
                    objtblServerGroups.GroupId = objtblDeviceGroups.GroupId;
                    //objtblServerGroups.Servers = objDeviceGroupViewModel.ServerGroupViewModel.Servers;
                    objtblServerGroups.CpuCritical = objDeviceGroupViewModel.ServerGroupViewModel.CpuCritical;
                    objtblServerGroups.CpuWarning = objDeviceGroupViewModel.ServerGroupViewModel.CpuWarning;
                    objtblServerGroups.MemoryCritical = objDeviceGroupViewModel.ServerGroupViewModel.MemoryCritical;
                    objtblServerGroups.MemoryWarning = objDeviceGroupViewModel.ServerGroupViewModel.MemoryWarning;
                    objtblServerGroups.NetworkCritical = objDeviceGroupViewModel.ServerGroupViewModel.NetworkCritical;
                    objtblServerGroups.NetworkWarning = objDeviceGroupViewModel.ServerGroupViewModel.NetworkWarning;
                    objtblServerGroups.DiskCritical = objDeviceGroupViewModel.ServerGroupViewModel.DiskCritical;
                    objtblServerGroups.DiskWarning = objDeviceGroupViewModel.ServerGroupViewModel.DiskWarning;
                    objtblServerGroups.CreatedBy = "System";
                    objtblServerGroups.Createdon = DateTime.Now;
                    objtblServerGroups.IsActive = true;


                    context.tblServerGroups.Add(objtblServerGroups);
                    context.SaveChanges();
                }
                
            }
        }

        public void UpdateGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            using (var context = new MonitoringContext())
            {
                var existingDeviceGroups = context.tblDeviceGroups.Where(k => k.GroupId == objDeviceGroupViewModel.GroupId).FirstOrDefault();

                if (existingDeviceGroups != null)
                {

                    existingDeviceGroups.GroupName = objDeviceGroupViewModel.GroupName;
                    existingDeviceGroups.UpdatedBy = "System";
                    existingDeviceGroups.UpdatedOn = DateTime.Now;

                    context.SaveChanges();

                    if (objDeviceGroupViewModel.DeviceTypeId == (int)DeviceTypes.Servers)
                    {
                        var existingServerGroups = context.tblServerGroups.Where(k => k.Id == objDeviceGroupViewModel.ServerGroupViewModel.Id).FirstOrDefault();

                        //existingServerGroups.Servers = objDeviceGroupViewModel.ServerGroupViewModel.Servers;
                        existingServerGroups.CpuCritical = objDeviceGroupViewModel.ServerGroupViewModel.CpuCritical;
                        existingServerGroups.CpuWarning = objDeviceGroupViewModel.ServerGroupViewModel.CpuWarning;
                        existingServerGroups.MemoryCritical = objDeviceGroupViewModel.ServerGroupViewModel.MemoryCritical;
                        existingServerGroups.MemoryWarning = objDeviceGroupViewModel.ServerGroupViewModel.MemoryWarning;
                        existingServerGroups.NetworkCritical = objDeviceGroupViewModel.ServerGroupViewModel.NetworkCritical;
                        existingServerGroups.NetworkWarning = objDeviceGroupViewModel.ServerGroupViewModel.NetworkWarning;
                        existingServerGroups.DiskCritical = objDeviceGroupViewModel.ServerGroupViewModel.DiskCritical;
                        existingServerGroups.DiskWarning = objDeviceGroupViewModel.ServerGroupViewModel.DiskWarning;
                        existingServerGroups.UpdatedBy = "System";
                        existingServerGroups.UpdatedOn = DateTime.Now;

                        context.SaveChanges();
                    }
                }

            }
        }

        public List<tblDeviceGroups> GetGroups()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDeviceGroups.Where(k => k.IsActive == true).ToList();
            }
        }

        public DeviceGroupViewModel GetGroupsbyId(int Id)
        {
            DeviceGroupViewModel objDeviceGroupViewModel = new DeviceGroupViewModel();
            ServerGroupViewModel objServerGroupViewModel = new ServerGroupViewModel();

            using (var context = new MonitoringContext())
            {
                var groups = context.tblDeviceGroups.Where(k => k.IsActive == true  && k.GroupId == Id).FirstOrDefault();
                objDeviceGroupViewModel.GroupId = groups.GroupId;
                objDeviceGroupViewModel.DeviceTypeId = groups.DeviceTypeId;
                objDeviceGroupViewModel.GroupName = groups.GroupName;

                var objServerGroups = context.tblServerGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objServerGroupViewModel.Id = objServerGroups.Id;
                objServerGroupViewModel.CpuCritical = objServerGroups.CpuCritical.HasValue ? objServerGroups.CpuCritical.Value : 0 ;
                objServerGroupViewModel.CpuWarning = objServerGroups.CpuWarning.HasValue ? objServerGroups.CpuWarning.Value : 0;
                objServerGroupViewModel.DiskCritical = objServerGroups.DiskCritical.HasValue ? objServerGroups.DiskCritical.Value : 0;
                objServerGroupViewModel.DiskWarning = objServerGroups.DiskWarning.HasValue ? objServerGroups.DiskWarning.Value : 0;
                objServerGroupViewModel.MemoryCritical = objServerGroups.MemoryCritical.HasValue ? objServerGroups.MemoryCritical.Value : 0;
                objServerGroupViewModel.MemoryWarning = objServerGroups.MemoryWarning.HasValue ? objServerGroups.MemoryWarning.Value : 0;

                objDeviceGroupViewModel.ServerGroupViewModel = objServerGroupViewModel;

            }

            return objDeviceGroupViewModel;
        }

        public List<tblServerGroups> GetServerGroups()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblServerGroups.Where(k => k.IsActive == true).ToList();
            }
        }

        //public void UpdateGroups(tblServerGroups objtblServerGroups)
        //{
        //    using (var context = new MonitoringContext())
        //    {
        //        var existingRecord = context.tblServerGroups.Where(k => k.GroupName == objtblServerGroups.GroupName && k.IsActive == true).FirstOrDefault();
        //        if (existingRecord != null)
        //        {
        //            existingRecord = objtblServerGroups;
        //            context.SaveChanges();
        //        }
        //    }
        //}

        public void RemoveGroups(int groupId)
        {
            using (var context = new MonitoringContext())
            {
                var existingRecord = context.tblDeviceGroups.Where(k => k.GroupId == groupId).FirstOrDefault();
                if (existingRecord != null)
                {
                    existingRecord.IsActive = false;
                    context.SaveChanges();
                }

                if(existingRecord.DeviceTypeId == (int)DeviceTypes.Servers)
                {
                    var existingthresholds = context.tblServerGroups.Where(k => k.GroupId == groupId).FirstOrDefault();
                    existingthresholds.IsActive = false;
                    context.SaveChanges();
                }
            }
        }

        public List<tblDashBoards> GetDashBoards()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDashBoards.Where(k => k.IsActive == true).ToList();
            }
        }

        public void CreateDashBoards(tblDashBoards objtblDashBoards)
        {
            using (var context = new MonitoringContext())
            {
                if (!context.tblDashBoards.Where(k => k.DashBoardName == objtblDashBoards.DashBoardName).Any())
                {
                    context.tblDashBoards.Add(objtblDashBoards);
                }

            }
        }

        public void UpdateDashBoards(tblDashBoards objtblDashBoards)
        {
            using (var context = new MonitoringContext())
            {
                var existingrecord = context.tblDashBoards.Where(k => k.DashBoardId == objtblDashBoards.DashBoardId).FirstOrDefault();
                existingrecord = objtblDashBoards;
                context.SaveChanges();
            }
        }

        public List<DashBoardSlideMapping> GetDashBoardSlideMapping()
        {
            using (var context = new MonitoringContext())
            {
                return context.DashBoardSlideMapping.Where(k => k.IsActive == true).ToList();
            }
        }

        public tblSlides GetSlideById(int slideId)
        {
            using (var context = new MonitoringContext())
            {
                return context.tblSlides.Where(k => k.IsActive == true && k.SlideId == slideId).FirstOrDefault();
            }
        }

        //public List<string> GetServers()
        //{
        //    using (var context = new MonitoringContext())
        //    {
        //        return context.tblDevices.Where(k => k.IsActive == true && k.DeviceTypeId == (int)DeviceTypes.Servers).Select(k=>k.DeviceName).ToList();
        //    }
        //}

        public void MapServerstoGroup(tblServerGroups objtblServerGroups)
        {
            using (var context = new MonitoringContext())
            {
                if (context.tblDeviceGroups.Where(k => k.GroupId == objtblServerGroups.GroupId).Any())
                {
                    context.tblServerGroups.Add(objtblServerGroups);
                }
            }
        }

        public void MapSlidetoDashBoard(DashBoardSlideMapping objDashBoardSlideMapping)
        {
            using (var context = new MonitoringContext())
            {
                if (!context.DashBoardSlideMapping.Where(k => k.SlideId == objDashBoardSlideMapping.SlideId).Any())
                {
                    context.DashBoardSlideMapping.Add(objDashBoardSlideMapping);
                }
            }
        }
    }
}