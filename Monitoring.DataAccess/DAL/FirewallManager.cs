using Monitoring.Common.CommonModels;
using MonitoringWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.DataAccess.DAL
{
    public class FirewallManager
    {
        public List<tblDeviceGroups> GetFirewallGroups()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDeviceGroups.Where(k => k.IsActive == true && k.DeviceTypeId == (int)DeviceTypes.Firewalls).ToList();
            }
        }

        public void CreateFirewallGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            using (var context = new MonitoringContext())
            {
                tblDeviceGroups objtblDeviceGroups = new tblDeviceGroups() { GroupName = objDeviceGroupViewModel.GroupName, DeviceTypeId = objDeviceGroupViewModel.DeviceTypeId, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.Now };
                context.tblDeviceGroups.Add(objtblDeviceGroups);
                context.SaveChanges();

                if (objDeviceGroupViewModel.DeviceTypeId == (int)DeviceTypes.Firewalls)
                {
                    tblFirewallGroups objtblFirewallGroups = new tblFirewallGroups();
                    objtblFirewallGroups.GroupId = objtblDeviceGroups.GroupId;
                    objtblFirewallGroups.CreatedBy = "System";
                    objtblFirewallGroups.CreatedOn = DateTime.Now;
                    objtblFirewallGroups.Interval = objDeviceGroupViewModel.FirewallGroupViewModel.Interval;
                    objtblFirewallGroups.IsActive = true;
                    objtblFirewallGroups.Retries = objDeviceGroupViewModel.FirewallGroupViewModel.Retries;

                    context.tblFirewallGroups.Add(objtblFirewallGroups);
                    context.SaveChanges();
                }
            }
        }

        public void UpdateFirewallGroups(DeviceGroupViewModel objDeviceGroupViewModel)
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

                    var existingRecord = context.tblFirewallGroups.Where(k => k.GroupId == objDeviceGroupViewModel.GroupId).FirstOrDefault();
                    existingRecord.Interval = objDeviceGroupViewModel.FirewallGroupViewModel.Interval;
                    existingRecord.Retries = objDeviceGroupViewModel.FirewallGroupViewModel.Retries;
                    existingRecord.UpdatedBy = "System";
                    existingRecord.UpdatedOn = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public void DeleteFirewallGroups(int groupId)
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

                if (existingRecord.DeviceTypeId == (int)DeviceTypes.Firewalls)
                {
                    var existingthresholds = context.tblFirewallGroups.Where(k => k.GroupId == groupId).FirstOrDefault();
                    existingthresholds.IsActive = false;
                    existingRecord.UpdatedBy = "System";
                    existingRecord.UpdatedOn = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public DeviceGroupViewModel GetFirewallGroupsId(int Id)
        {
            DeviceGroupViewModel objDeviceGroupViewModel = new DeviceGroupViewModel();
            FirewallGroupViewModel objFirewallGroupViewModel = new FirewallGroupViewModel();

            using (var context = new MonitoringContext())
            {
                var groups = context.tblDeviceGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objDeviceGroupViewModel.GroupId = groups.GroupId;
                objDeviceGroupViewModel.DeviceTypeId = groups.DeviceTypeId;
                objDeviceGroupViewModel.GroupName = groups.GroupName;

                var objFirewallGroups = context.tblFirewallGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objFirewallGroupViewModel.Id = objFirewallGroups.Id;
                objFirewallGroupViewModel.Interval = objFirewallGroups.Interval.HasValue? objFirewallGroups.Interval.Value:0;
                objFirewallGroupViewModel.Retries = objFirewallGroups.Retries.HasValue? objFirewallGroups.Retries.Value:0;
                if (!string.IsNullOrEmpty(objFirewallGroups.Devices))
                {
                    objFirewallGroupViewModel.SelectedDevices = objFirewallGroups.Devices.Split(',').ToList();
                }

                objDeviceGroupViewModel.FirewallGroupViewModel = objFirewallGroupViewModel;

            }

            return objDeviceGroupViewModel;

        }

        public void MapFireWallDevicetoGroup(tblFirewallGroups objtblFirewallGroups)
        {
            using (var context = new MonitoringContext())
            {
                if (context.tblDeviceGroups.Where(k => k.GroupId == objtblFirewallGroups.GroupId).Any())
                {
                    var existingrecord = context.tblFirewallGroups.Where(k => k.GroupId == objtblFirewallGroups.GroupId).FirstOrDefault();
                    existingrecord.Devices = objtblFirewallGroups.Devices;
                    context.SaveChanges();
                }
            }
        }

        public List<PingDeviceDetails> GetFirewallDetails()
        {
            List<PingDeviceDetails> objDevices = new List<PingDeviceDetails>();

            using (var context = new MonitoringContext())
            {
                objDevices = (from p in context.tblDevices.Where(k => k.IsActive == true)
                              join q in context.tblDeviceTypes.Where(k => k.CanPingable.Value == true && k.IsActive == true)
                                on p.DeviceTypeId equals q.AssetTypeId
                              from r in context.tblFirewallGroups.Where(k => k.IsActive == true && k.Devices.Contains(p.DeviceName))
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
