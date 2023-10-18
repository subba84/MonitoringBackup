using Monitoring.Common.CommonModels;
using MonitoringWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.DataAccess.DAL
{
    public class NetworkSwitchManager
    {
        public List<tblDeviceGroups> GetNetworkSwitchGroups()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDeviceGroups.Where(k => k.IsActive == true && k.DeviceTypeId == (int)DeviceTypes.NetworkSwitches).ToList();
            }
        }

        public void CreateNetworkSwitchGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            using (var context = new MonitoringContext())
            {
                tblDeviceGroups objtblDeviceGroups = new tblDeviceGroups() { GroupName = objDeviceGroupViewModel.GroupName, DeviceTypeId = objDeviceGroupViewModel.DeviceTypeId, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.Now };
                context.tblDeviceGroups.Add(objtblDeviceGroups);
                context.SaveChanges();

                if (objDeviceGroupViewModel.DeviceTypeId == (int)DeviceTypes.NetworkSwitches)
                {
                    tblNetworkswitchGroups objtblNetworkswitchGroups = new tblNetworkswitchGroups();
                    objtblNetworkswitchGroups.GroupId = objtblDeviceGroups.GroupId;
                    objtblNetworkswitchGroups.CreatedBy = "System";
                    objtblNetworkswitchGroups.CreatedOn = DateTime.Now;
                    objtblNetworkswitchGroups.Interval = objDeviceGroupViewModel.NetworkSwitchGroupViewModel.Interval;
                    objtblNetworkswitchGroups.IsActive = true;
                    objtblNetworkswitchGroups.Retries = objDeviceGroupViewModel.NetworkSwitchGroupViewModel.Retries;

                    context.tblNetworkswitchGroups.Add(objtblNetworkswitchGroups);
                    context.SaveChanges();
                }
            }
        }

        public void UpdateNetworkSwitchGroups(DeviceGroupViewModel objDeviceGroupViewModel)
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

                    var existingRecord = context.tblNetworkswitchGroups.Where(k => k.GroupId == objDeviceGroupViewModel.GroupId).FirstOrDefault();
                    existingRecord.Interval = objDeviceGroupViewModel.NetworkSwitchGroupViewModel.Interval;
                    existingRecord.Retries = objDeviceGroupViewModel.NetworkSwitchGroupViewModel.Retries;
                    existingRecord.UpdatedBy = "System";
                    existingRecord.UpdatedOn = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public void DeleteNetworkSwitchGroups(int groupId)
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

                if (existingRecord.DeviceTypeId == (int)DeviceTypes.NetworkSwitches)
                {
                    var existingthresholds = context.tblNetworkswitchGroups.Where(k => k.GroupId == groupId).FirstOrDefault();
                    existingthresholds.IsActive = false;
                    existingRecord.UpdatedBy = "System";
                    existingRecord.UpdatedOn = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public DeviceGroupViewModel GetNetworkSwitchGroupsId(int Id)
        {
            DeviceGroupViewModel objDeviceGroupViewModel = new DeviceGroupViewModel();
            NetworkSwitchGroupViewModel objNetworkSwitchGroupViewModel = new NetworkSwitchGroupViewModel();

            using (var context = new MonitoringContext())
            {
                var groups = context.tblDeviceGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objDeviceGroupViewModel.GroupId = groups.GroupId;
                objDeviceGroupViewModel.DeviceTypeId = groups.DeviceTypeId;
                objDeviceGroupViewModel.GroupName = groups.GroupName;

                var objNetworkswitchGroups = context.tblNetworkswitchGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objNetworkSwitchGroupViewModel.Id = objNetworkswitchGroups.Id;
                objNetworkSwitchGroupViewModel.Interval = objNetworkswitchGroups.Interval.HasValue? objNetworkswitchGroups.Interval.Value:0;
                objNetworkSwitchGroupViewModel.Retries = objNetworkswitchGroups.Retries.HasValue? objNetworkswitchGroups.Retries.Value:0;
                if (!string.IsNullOrEmpty(objNetworkswitchGroups.Devices))
                {
                    objNetworkSwitchGroupViewModel.SelectedDevices = objNetworkswitchGroups.Devices.Split(',').ToList();
                }

                objDeviceGroupViewModel.NetworkSwitchGroupViewModel = objNetworkSwitchGroupViewModel;

            }

            return objDeviceGroupViewModel;

        }

        public void MapNetworkSwitchGroups(tblNetworkswitchGroups objtblNetworkswitchGroups)
        {
            using (var context = new MonitoringContext())
            {
                if (context.tblDeviceGroups.Where(k => k.GroupId == objtblNetworkswitchGroups.GroupId).Any())
                {
                    var existingrecord = context.tblNetworkswitchGroups.Where(k => k.GroupId == objtblNetworkswitchGroups.GroupId).FirstOrDefault();
                    existingrecord.Devices = objtblNetworkswitchGroups.Devices;
                    context.SaveChanges();
                }
            }
        }

        public List<PingDeviceDetails> GetNetworkSwitchDetails()
        {
            List<PingDeviceDetails> objDevices = new List<PingDeviceDetails>();

            using (var context = new MonitoringContext())
            {
                objDevices = (from p in context.tblDevices.Where(k => k.IsActive == true)
                              join q in context.tblDeviceTypes.Where(k => k.CanPingable.Value == true && k.IsActive == true)
                                on p.DeviceTypeId equals q.AssetTypeId
                              from r in context.tblNetworkswitchGroups.Where(k => k.IsActive == true && k.Devices.Contains(p.DeviceName))
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
