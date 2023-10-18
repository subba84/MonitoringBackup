using Monitoring.Common.CommonModels;
using MonitoringWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.DataAccess.DAL
{
    public class AccessPointManager
    {
        public List<tblDeviceGroups> GetAccessPointGroups()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDeviceGroups.Where(k => k.IsActive == true && k.DeviceTypeId == (int)DeviceTypes.AccessPoints).ToList();
            }
        }

        public void CreateAccessPointGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            using (var context = new MonitoringContext())
            {
                tblDeviceGroups objtblDeviceGroups = new tblDeviceGroups() { GroupName = objDeviceGroupViewModel.GroupName, DeviceTypeId = objDeviceGroupViewModel.DeviceTypeId, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.Now };
                context.tblDeviceGroups.Add(objtblDeviceGroups);
                context.SaveChanges();

                if (objDeviceGroupViewModel.DeviceTypeId == (int)DeviceTypes.AccessPoints)
                {
                    tblAccessPointGroups objtblAccessPointGroups = new tblAccessPointGroups();
                    objtblAccessPointGroups.GroupId = objtblDeviceGroups.GroupId;
                    objtblAccessPointGroups.CreatedBy = "System";
                    objtblAccessPointGroups.CreatedOn = DateTime.Now;
                    objtblAccessPointGroups.Interval = objDeviceGroupViewModel.AccessPointGroupViewModel.Interval;
                    objtblAccessPointGroups.IsActive = true;
                    objtblAccessPointGroups.Retries = objDeviceGroupViewModel.AccessPointGroupViewModel.Retries;

                    context.tblAccessPointGroups.Add(objtblAccessPointGroups);
                    context.SaveChanges();
                }
            }
        }

        public void UpdateAccessPointGroups(DeviceGroupViewModel objDeviceGroupViewModel)
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

                    var existingRecord = context.tblAccessPointGroups.Where(k => k.GroupId == objDeviceGroupViewModel.GroupId).FirstOrDefault();
                    existingRecord.Interval = objDeviceGroupViewModel.AccessPointGroupViewModel.Interval;
                    existingRecord.Retries = objDeviceGroupViewModel.AccessPointGroupViewModel.Retries;
                    existingRecord.UpdatedBy = "System";
                    existingRecord.UpdatedOn = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public void DeleteAccessPointGroups(int groupId)
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

                if (existingRecord.DeviceTypeId == (int)DeviceTypes.AccessPoints)
                {
                    var existingthresholds = context.tblAccessPointGroups.Where(k => k.GroupId == groupId).FirstOrDefault();
                    existingthresholds.IsActive = false;
                    existingRecord.UpdatedBy = "System";
                    existingRecord.UpdatedOn = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public DeviceGroupViewModel GetAccessPointGroupsId(int Id)
        {
            DeviceGroupViewModel objDeviceGroupViewModel = new DeviceGroupViewModel();
            AccessPointGroupViewModel objAccessPointGroupViewModel = new AccessPointGroupViewModel();

            using (var context = new MonitoringContext())
            {
                var groups = context.tblDeviceGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objDeviceGroupViewModel.GroupId = groups.GroupId;
                objDeviceGroupViewModel.DeviceTypeId = groups.DeviceTypeId;
                objDeviceGroupViewModel.GroupName = groups.GroupName;

                var objtblAccessPointGroups = context.tblAccessPointGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objAccessPointGroupViewModel.Id = objtblAccessPointGroups.Id;
                objAccessPointGroupViewModel.Interval = objtblAccessPointGroups.Interval.HasValue? objtblAccessPointGroups.Interval.Value:0;
                objAccessPointGroupViewModel.Retries = objtblAccessPointGroups.Retries.HasValue? objtblAccessPointGroups.Retries.Value:0;
                if (!string.IsNullOrEmpty(objtblAccessPointGroups.Devices))
                {
                    objAccessPointGroupViewModel.SelectedDevices = objtblAccessPointGroups.Devices.Split(',').ToList();
                }

                objDeviceGroupViewModel.AccessPointGroupViewModel = objAccessPointGroupViewModel;
            }

            return objDeviceGroupViewModel;

        }

        public void MapAccessPointstoGroup(tblAccessPointGroups objtblAccessPointGroups)
        {
            using (var context = new MonitoringContext())
            {
                if (context.tblAccessPointGroups.Where(k => k.GroupId == objtblAccessPointGroups.GroupId).Any())
                {
                    var existingrecord = context.tblAccessPointGroups.Where(k => k.GroupId == objtblAccessPointGroups.GroupId).FirstOrDefault();
                    existingrecord.Devices = objtblAccessPointGroups.Devices;
                    context.SaveChanges();
                }
            }
        }

        public List<PingDeviceDetails> GetAccessPointDetails()
        {
            List<PingDeviceDetails> objDevices = new List<PingDeviceDetails>();

            using (var context = new MonitoringContext())
            {
                objDevices = (from p in context.tblDevices.Where(k => k.IsActive == true)
                              join q in context.tblDeviceTypes.Where(k => k.CanPingable.Value == true && k.IsActive == true)
                                on p.DeviceTypeId equals q.AssetTypeId
                              from r in context.tblAccessPointGroups.Where(k => k.IsActive == true && k.Devices.Contains(p.DeviceName))
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
