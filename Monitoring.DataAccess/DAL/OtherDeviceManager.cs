using Monitoring.Common.CommonModels;
using MonitoringWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.DataAccess.DAL
{
    public class OtherDeviceManager
    {
        public List<tblDeviceGroups> GetOtherDeviceGroups()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDeviceGroups.Where(k => k.IsActive == true && k.DeviceTypeId == (int)DeviceTypes.OtherDevices).ToList();
            }
        }

        public void CreateOtherDeviceGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            using (var context = new MonitoringContext())
            {
                tblDeviceGroups objtblDeviceGroups = new tblDeviceGroups() { GroupName = objDeviceGroupViewModel.GroupName, DeviceTypeId = objDeviceGroupViewModel.DeviceTypeId, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.Now };
                context.tblDeviceGroups.Add(objtblDeviceGroups);
                context.SaveChanges();

                if (objDeviceGroupViewModel.DeviceTypeId == (int)DeviceTypes.OtherDevices)
                {
                    tblOtherDeviceGroups objtblOtherDeviceGroups = new tblOtherDeviceGroups();
                    objtblOtherDeviceGroups.GroupId = objtblDeviceGroups.GroupId;
                    objtblOtherDeviceGroups.CreatedBy = "System";
                    objtblOtherDeviceGroups.CreatedOn = DateTime.Now;
                    objtblOtherDeviceGroups.Interval = objDeviceGroupViewModel.otherDeviceGroupViewModel.Interval;
                    objtblOtherDeviceGroups.IsActive = true;
                    objtblOtherDeviceGroups.Retries = objDeviceGroupViewModel.otherDeviceGroupViewModel.Retries;

                    context.tblOtherDeviceGroups.Add(objtblOtherDeviceGroups);
                    context.SaveChanges();
                }
            }
        }

        public void UpdateOtherDeviceGroups(DeviceGroupViewModel objDeviceGroupViewModel)
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

                    var existingRecord = context.tblOtherDeviceGroups.Where(k => k.GroupId == objDeviceGroupViewModel.GroupId).FirstOrDefault();
                    existingRecord.Interval = objDeviceGroupViewModel.otherDeviceGroupViewModel.Interval;
                    existingRecord.Retries = objDeviceGroupViewModel.otherDeviceGroupViewModel.Retries;
                    existingRecord.UpdatedBy = "System";
                    existingRecord.UpdatedOn = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public void DeleteOtherDeviceGroups(int groupId)
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

                if (existingRecord.DeviceTypeId == (int)DeviceTypes.OtherDevices)
                {
                    var existingthresholds = context.tblOtherDeviceGroups.Where(k => k.GroupId == groupId).FirstOrDefault();
                    existingthresholds.IsActive = false;
                    existingRecord.UpdatedBy = "System";
                    existingRecord.UpdatedOn = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public DeviceGroupViewModel GetOtherDeviceGroupsId(int Id)
        {
            DeviceGroupViewModel objDeviceGroupViewModel = new DeviceGroupViewModel();
            OtherDeviceGroupViewModel objOtherDeviceGroupViewModel = new OtherDeviceGroupViewModel();

            using (var context = new MonitoringContext())
            {
                var groups = context.tblDeviceGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objDeviceGroupViewModel.GroupId = groups.GroupId;
                objDeviceGroupViewModel.DeviceTypeId = groups.DeviceTypeId;
                objDeviceGroupViewModel.GroupName = groups.GroupName;

                var objOtherDeviceGroups = context.tblOtherDeviceGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objOtherDeviceGroupViewModel.Id = objOtherDeviceGroups.Id;
                objOtherDeviceGroupViewModel.Interval = objOtherDeviceGroups.Interval.HasValue? objOtherDeviceGroups.Interval.Value:0;
                objOtherDeviceGroupViewModel.Retries = objOtherDeviceGroups.Retries.HasValue? objOtherDeviceGroups.Retries.Value:0;
                if (!string.IsNullOrEmpty(objOtherDeviceGroups.Devices))
                {
                    objOtherDeviceGroupViewModel.SelectedDevices = objOtherDeviceGroups.Devices.Split(',').ToList();
                }

                objDeviceGroupViewModel.otherDeviceGroupViewModel = objOtherDeviceGroupViewModel;

            }

            return objDeviceGroupViewModel;

        }

        public void MapOtherDevicetoGroup(tblOtherDeviceGroups objtblOtherDeviceGroups)
        {
            using (var context = new MonitoringContext())
            {
                if (context.tblDeviceGroups.Where(k => k.GroupId == objtblOtherDeviceGroups.GroupId).Any())
                {
                    var existingrecord = context.tblOtherDeviceGroups.Where(k => k.GroupId == objtblOtherDeviceGroups.GroupId).FirstOrDefault();
                    existingrecord.Devices = objtblOtherDeviceGroups.Devices;
                    context.SaveChanges();
                }
            }
        }

        public List<PingDeviceDetails> GetOtherDeviceDetails()
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
