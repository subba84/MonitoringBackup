using Monitoring.Common;
using MonitoringWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Monitoring.Common.Logging;
using Monitoring.Common.CommonModels;
using System.Data.Entity.Validation;

namespace MonitoringWebService.DAL
{
    public class MonitoringManager
    {
        #region Common

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
                if (existingrecord != null)
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

        public List<tblDevices> GetDeviceById(int deviceTypeId)
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

        #endregion

        #region Slides

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

        public List<tblSlides> GetSlidesbydeviceType(int Id)
        {
            using (var context = new MonitoringContext())
            {
                return context.tblSlides.Where(k => k.IsActive == true && k.DeviceTypeId == Id).ToList();
            }
        }

        public void UpdateSlides(tblSlides objtblSlides)
        {
            using (var context = new MonitoringContext())
            {
                var existingRecord = context.tblSlides.Where(k => k.SlideId == objtblSlides.SlideId && k.IsActive == true).FirstOrDefault();
                if (existingRecord != null)
                {
                    existingRecord.SlideName = objtblSlides.SlideName;
                    existingRecord.Devices = objtblSlides.Devices;
                    existingRecord.UpdatedBy = Constants.CreatedBy;
                    existingRecord.UpdatedOn = DateTime.Now;
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

        public tblSlides GetSlideById(int slideId)
        {
            using (var context = new MonitoringContext())
            {
                return context.tblSlides.Where(k => k.IsActive == true && k.SlideId == slideId).FirstOrDefault();
            }
        }

        #endregion

        #region Groups

        public void CreateGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            try
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
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (Exception ex)
            {

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
                return context.tblDeviceGroups.Where(k => k.IsActive == true && k.DeviceTypeId == (int)DeviceTypes.Servers).ToList();
            }
        }

        public DeviceGroupViewModel GetGroupsbyId(int Id)
        {
            DeviceGroupViewModel objDeviceGroupViewModel = new DeviceGroupViewModel();
            ServerGroupViewModel objServerGroupViewModel = new ServerGroupViewModel();

            using (var context = new MonitoringContext())
            {
                var groups = context.tblDeviceGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objDeviceGroupViewModel.GroupId = groups.GroupId;
                objDeviceGroupViewModel.DeviceTypeId = groups.DeviceTypeId;
                objDeviceGroupViewModel.GroupName = groups.GroupName;

                var objServerGroups = context.tblServerGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objServerGroupViewModel.Id = objServerGroups.Id;
                objServerGroupViewModel.CpuCritical = objServerGroups.CpuCritical.HasValue ? objServerGroups.CpuCritical.Value : 0;
                objServerGroupViewModel.CpuWarning = objServerGroups.CpuWarning.HasValue ? objServerGroups.CpuWarning.Value : 0;
                objServerGroupViewModel.DiskCritical = objServerGroups.DiskCritical.HasValue ? objServerGroups.DiskCritical.Value : 0;
                objServerGroupViewModel.DiskWarning = objServerGroups.DiskWarning.HasValue ? objServerGroups.DiskWarning.Value : 0;
                objServerGroupViewModel.MemoryCritical = objServerGroups.MemoryCritical.HasValue ? objServerGroups.MemoryCritical.Value : 0;
                objServerGroupViewModel.MemoryWarning = objServerGroups.MemoryWarning.HasValue ? objServerGroups.MemoryWarning.Value : 0;
                objServerGroupViewModel.NetworkCritical = objServerGroups.NetworkCritical.HasValue ? objServerGroups.NetworkCritical.Value : 0;
                objServerGroupViewModel.NetworkWarning = objServerGroups.NetworkWarning.HasValue ? objServerGroups.NetworkWarning.Value : 0;

                objDeviceGroupViewModel.ServerGroupViewModel = objServerGroupViewModel;

            }

            return objDeviceGroupViewModel;
        }

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

                if (existingRecord.DeviceTypeId == (int)DeviceTypes.Servers)
                {
                    var existingthresholds = context.tblServerGroups.Where(k => k.GroupId == groupId).FirstOrDefault();
                    existingthresholds.IsActive = false;
                    context.SaveChanges();
                }
            }
        }

        public void MapServerstoGroup(tblServerGroups objtblServerGroups)
        {
            using (var context = new MonitoringContext())
            {
                if (context.tblServerGroups.Where(k => k.GroupId == objtblServerGroups.GroupId).Any())
                {
                    var existingrecord = context.tblServerGroups.Where(k => k.GroupId == objtblServerGroups.GroupId).FirstOrDefault();
                    existingrecord.Servers = objtblServerGroups.Servers;
                    context.SaveChanges();
                }
            }
        }

        #endregion

        #region DashBoards

        public List<tblDashBoards> GetDashBoards()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDashBoards.Where(k => k.IsActive == true).ToList();
            }
        }

        public tblDashBoards GetDashBoardById(int dashBoardId)
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDashBoards.Where(k => k.IsActive == true && k.DashBoardId == dashBoardId).FirstOrDefault();
            }
        }

        public void CreateDashBoards(AllDashBoardViewModel objDashBoard)
        {
            using (var context = new MonitoringContext())
            {
                if (!context.tblDashBoards.Where(k => k.DashBoardName == objDashBoard.DashBoardName).Any())
                {
                    tblDashBoards objtblDashBoards = new tblDashBoards();
                    objtblDashBoards.DashBoardName = objDashBoard.DashBoardName;
                    objtblDashBoards.IsActive = true;
                    objtblDashBoards.CreatedOn = DateTime.Now;
                    objtblDashBoards.CreatedBy = Constants.CreatedBy;
                    context.tblDashBoards.Add(objtblDashBoards);
                    context.SaveChanges();


                    DashBoardSlideMapping dashBoardSlideMapping = new DashBoardSlideMapping();
                    dashBoardSlideMapping.DashBoardId = objtblDashBoards.DashBoardId;
                    dashBoardSlideMapping.Slides = string.Join(",", objDashBoard.SelectedSlides);
                    dashBoardSlideMapping.CreatedBy = Constants.CreatedBy;
                    dashBoardSlideMapping.CreatedOn = DateTime.Now;
                    dashBoardSlideMapping.IsActive = true;
                    context.DashBoardSlideMapping.Add(dashBoardSlideMapping);
                    context.SaveChanges();

                }

            }
        }

        public void UpdateDashBoards(AllDashBoardViewModel objDashBoard)
        {
            using (var context = new MonitoringContext())
            {
                var existingrecord = context.tblDashBoards.Where(k => k.DashBoardId == objDashBoard.DashBoardId).FirstOrDefault();
                existingrecord.DashBoardName = objDashBoard.DashBoardName;
                existingrecord.UpdatedBy = Constants.CreatedBy;
                existingrecord.UpdatedOn = DateTime.Now;
                context.SaveChanges();

                var existingmappingrecord = context.DashBoardSlideMapping.Where(k => k.DashBoardId == objDashBoard.DashBoardId).FirstOrDefault();
                existingmappingrecord.Slides = string.Join(",", objDashBoard.SelectedSlides);
                existingmappingrecord.UpdatedBy = Constants.CreatedBy;
                existingmappingrecord.UpdatedOn = DateTime.Now;
                context.SaveChanges();
            }
        }

        public DashBoardSlideMapping GetDashBoardSlideMapping(int dashBoardId)
        {
            using (var context = new MonitoringContext())
            {
                return context.DashBoardSlideMapping.Where(k => k.IsActive == true && k.DashBoardId == dashBoardId).FirstOrDefault();
            }
        }

        public List<DashBoardSlideMapping> GetDashBoardSlideMapping()
        {
            using (var context = new MonitoringContext())
            {
                return context.DashBoardSlideMapping.Where(k => k.IsActive == true).ToList();
            }
        }

        public void removeDashBoards(int dashBoardId)
        {
            using (var context = new MonitoringContext())
            {
                var existingRecord = context.tblDashBoards.Where(k => k.DashBoardId == dashBoardId).FirstOrDefault();
                if (existingRecord != null)
                {
                    existingRecord.IsActive = false;
                    context.SaveChanges();
                }


                var existingmappingrecord = context.DashBoardSlideMapping.Where(k => k.DashBoardId == dashBoardId).FirstOrDefault();
                existingmappingrecord.IsActive = false;
                context.SaveChanges();
            }
        }

        #endregion

        public tblServerGroups GetServerGroupById(int groupId)
        {
            using (var context = new MonitoringContext())
            {
                return context.tblServerGroups.Where(k => k.IsActive == true && k.GroupId == groupId).FirstOrDefault();
            }
        }

        public List<tblServerGroups> GetServerGroups()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblServerGroups.Where(k => k.IsActive == true).ToList();
            }
        }

        public List<tblDevices> GetPingDevices()
        {
            List<tblDevices> objDevices = new List<tblDevices>();
            using (var context = new MonitoringContext())
            {
                objDevices = (from p in context.tblDevices.Where(k => k.IsActive == true)
                              join q in context.tblDeviceTypes.Where(k => k.CanPingable.Value == true && k.IsActive == true)
                                on p.DeviceTypeId equals q.AssetTypeId
                              select new tblDevices()
                              {
                                  DeviceId = p.DeviceId,
                                  DeviceName = p.DeviceName,
                                  CategoryId = p.CategoryId,
                                  CreatedBy = p.CreatedBy,
                                  CreatedOn = p.CreatedOn,
                                  DeviceTypeId = p.DeviceTypeId,
                                  DisplayName = p.DisplayName,
                                  IpAddress = p.IpAddress,
                                  IsActive = p.IsActive,
                                  Location = p.Location,
                                  ManagedTypeId = p.ManagedTypeId,
                                  Password = p.Password,
                                  Status = p.Status,
                                  SubCategoryId = p.SubCategoryId,
                                  //UpdatedBy = p.UpdatedBy,
                                  //UpdatedOn = p.UpdatedOn,
                                  UserId = p.UserId,
                                  VendorId = p.VendorId
                              }
                             ).ToList();
            }

            return objDevices;
        }

        public void UpdateDeviceStatus(int deviceId, int status)
        {
            using (var context = new MonitoringContext())
            {
                var device = context.tblDevices.Where(k => k.DeviceId == deviceId && k.IsActive == true).FirstOrDefault();

                if (device != null)
                {
                    device.Status = status;
                    context.SaveChanges();
                }
            }
        }

        public List<PingDeviceDetails> GetOtherDevicesDetails()
        {
            List<PingDeviceDetails> objDevices = new List<PingDeviceDetails>();

            using (var context = new MonitoringContext())
            {
                objDevices = (from p in context.tblDevices.Where(k => k.IsActive == true)
                              join q in context.tblDeviceTypes.Where(k => k.CanPingable.Value == true && k.IsActive == true)
                                on p.DeviceTypeId equals q.AssetTypeId
                              from r in context.tblOtherDeviceGroups.Where(k => k.IsActive == true && k.Devices.Contains(p.DeviceName))
                              select new PingDeviceDetails()
                              {
                                  DeviceName = p.DeviceName,
                                  DeviceId = p.DeviceId,
                                  Retries = r.Retries.HasValue ? r.Retries.Value : 0,
                                  Interval = r.Interval.HasValue ? r.Interval.Value : 0,
                                  IPAddress = p.IpAddress
                              }
                             ).ToList();
            }

            return objDevices;
        }
    }
}
