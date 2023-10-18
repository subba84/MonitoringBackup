using Monitoring.Common.CommonModels;
using MonitoringWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.DataAccess.DAL
{
    public class CctvCameraManager
    {
        public List<tblDeviceGroups> GetCCTVCameraGroups()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDeviceGroups.Where(k => k.IsActive == true && k.DeviceTypeId == (int)DeviceTypes.CCTVCameras).ToList();
            }
        }

        public void CreateCctvCameraGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            using (var context = new MonitoringContext())
            {
                tblDeviceGroups objtblDeviceGroups = new tblDeviceGroups() { GroupName = objDeviceGroupViewModel.GroupName, DeviceTypeId = objDeviceGroupViewModel.DeviceTypeId, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.Now };
                context.tblDeviceGroups.Add(objtblDeviceGroups);
                context.SaveChanges();

                if (objDeviceGroupViewModel.DeviceTypeId == (int)DeviceTypes.CCTVCameras)
                {
                    tblCctvCameraGroups objtblCctvCameraGroups = new tblCctvCameraGroups();
                    objtblCctvCameraGroups.GroupId = objtblDeviceGroups.GroupId;
                    objtblCctvCameraGroups.CreatedBy = "System";
                    objtblCctvCameraGroups.CreatedOn = DateTime.Now;
                    objtblCctvCameraGroups.Interval = objDeviceGroupViewModel.CctvCameraGroupViewModel.Interval;
                    objtblCctvCameraGroups.IsActive = true;
                    objtblCctvCameraGroups.Retries = objDeviceGroupViewModel.CctvCameraGroupViewModel.Retries;

                    context.tblCctvCameraGroups.Add(objtblCctvCameraGroups);
                    context.SaveChanges();
                }
            }
        }

        public void UpdateCctvCameraGroups(DeviceGroupViewModel objDeviceGroupViewModel)
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

                    var existingRecord = context.tblCctvCameraGroups.Where(k => k.GroupId == objDeviceGroupViewModel.GroupId).FirstOrDefault();
                    existingRecord.Interval = objDeviceGroupViewModel.CctvCameraGroupViewModel.Interval;
                    existingRecord.Retries = objDeviceGroupViewModel.CctvCameraGroupViewModel.Retries;
                    existingRecord.UpdatedBy = "System";
                    existingRecord.UpdatedOn = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public void DeleteCctvCameraGroups(int groupId)
        {
            using (var context = new MonitoringContext())
            {
                var existingRecord = context.tblDeviceGroups.Where(k => k.GroupId == groupId).FirstOrDefault();
                if (existingRecord != null)
                {
                    existingRecord.IsActive = false;
                    existingRecord.UpdatedBy = "System";
                    existingRecord.UpdatedOn = DateTime.Now;
                    context.SaveChanges();
                }

                if (existingRecord.DeviceTypeId == (int)DeviceTypes.CCTVCameras)
                {
                    var existingthresholds = context.tblCctvCameraGroups.Where(k => k.GroupId == groupId).FirstOrDefault();
                    existingthresholds.IsActive = false;
                    existingRecord.UpdatedBy = "System";
                    existingRecord.UpdatedOn = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public DeviceGroupViewModel GetCctvCameraGroupsId(int Id)
        {
            DeviceGroupViewModel objDeviceGroupViewModel = new DeviceGroupViewModel();
            CctvCameraGroupViewModel objCctvCameraGroupViewModel = new CctvCameraGroupViewModel();

            using (var context = new MonitoringContext())
            {
                var groups = context.tblDeviceGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objDeviceGroupViewModel.GroupId = groups.GroupId;
                objDeviceGroupViewModel.DeviceTypeId = groups.DeviceTypeId;
                objDeviceGroupViewModel.GroupName = groups.GroupName;

                var objCctvCameraGroups = context.tblCctvCameraGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objCctvCameraGroupViewModel.Id = objCctvCameraGroups.Id;
                objCctvCameraGroupViewModel.Interval = objCctvCameraGroups.Interval.HasValue? objCctvCameraGroups.Interval.Value:0;
                objCctvCameraGroupViewModel.Retries = objCctvCameraGroups.Retries.HasValue? objCctvCameraGroups.Retries.Value:0;
                if (!string.IsNullOrEmpty(objCctvCameraGroups.Devices))
                {
                    objCctvCameraGroupViewModel.SelectedDevices = objCctvCameraGroups.Devices.Split(',').ToList();
                }

                objDeviceGroupViewModel.CctvCameraGroupViewModel = objCctvCameraGroupViewModel;

            }

            return objDeviceGroupViewModel;

        }

        public void MapCctvCamerastoGroup(tblCctvCameraGroups objtblCctvCameraGroups)
        {
            using (var context = new MonitoringContext())
            {
                if (context.tblDeviceGroups.Where(k => k.GroupId == objtblCctvCameraGroups.GroupId).Any())
                {
                    var existingrecord = context.tblCctvCameraGroups.Where(k => k.GroupId == objtblCctvCameraGroups.GroupId).FirstOrDefault();
                    existingrecord.Devices = objtblCctvCameraGroups.Devices;
                    context.SaveChanges();
                }
            }
        }

        public List<PingDeviceDetails> GetCctvCameraDetails()
        {
            List<PingDeviceDetails> objDevices = new List<PingDeviceDetails>();

            using (var context = new MonitoringContext())
            {
                objDevices = (from p in context.tblDevices.Where(k => k.IsActive == true)
                              join q in context.tblDeviceTypes.Where(k => k.CanPingable.Value == true && k.IsActive == true)
                                on p.DeviceTypeId equals q.AssetTypeId
                              from r in context.tblCctvCameraGroups.Where(k => k.IsActive == true && k.Devices.Contains(p.DeviceName))
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
