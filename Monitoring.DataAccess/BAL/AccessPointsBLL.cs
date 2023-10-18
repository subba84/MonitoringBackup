using Monitoring.Common;
using Monitoring.Common.CommonModels;
using Monitoring.Common.CommonModels.ViewModels;
using Monitoring.DataAccess.DAL;
using MonitoringWebService.DAL;
using MonitoringWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.DataAccess.BAL
{
    public class AccessPointsBLL
    {
        AccessPointManager objAccessPointManager = new AccessPointManager();

        public List<GroupViewModel> GetAccessPointGroups()
        {
            List<tblDeviceGroups> objlsttblServerGroups = new List<tblDeviceGroups>();
            List<GroupViewModel> objlstGroupViewModel = new List<GroupViewModel>();
            objlsttblServerGroups = objAccessPointManager.GetAccessPointGroups();

            objlsttblServerGroups.ForEach(x => { objlstGroupViewModel.Add(new GroupViewModel() { GroupId = x.GroupId, DeviceTypeName = ((DeviceTypes)x.DeviceTypeId).ToString(), GroupName = x.GroupName }); });

            return objlstGroupViewModel;
        }

        public void CreateAccessPointGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            objAccessPointManager.CreateAccessPointGroups(objDeviceGroupViewModel);
        }

        public void UpdateAccessPointGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            objAccessPointManager.UpdateAccessPointGroups(objDeviceGroupViewModel);
        }

        public void DeleteAccessPointGroups(int groupId)
        {
            objAccessPointManager.DeleteAccessPointGroups(groupId);
        }

        public DeviceGroupViewModel GetAccessPointGroupsId(int Id)
        {
            return objAccessPointManager.GetAccessPointGroupsId(Id);

        }

        public void MapAccessPointstoGroup(AccessPointGroupViewModel objAccessPointGroupViewModel)
        {
            tblAccessPointGroups objtblAccessPointGroups = new tblAccessPointGroups();
            objtblAccessPointGroups.GroupId = objAccessPointGroupViewModel.GroupId;
            objtblAccessPointGroups.Devices = string.Join(",", objAccessPointGroupViewModel.SelectedDevices);

            objAccessPointManager.MapAccessPointstoGroup(objtblAccessPointGroups);
        }

        public List<PingDeviceDetails> GetAccessPointDetails()
        {
            return objAccessPointManager.GetAccessPointDetails();
        }

        public AccessPointGroupViewModel GetAccessPointGroupDetailsById(int Id)
        {
            AccessPointGroupViewModel objAccessPointGroupViewModel = new AccessPointGroupViewModel();

            List<tblDevices> objOtherDevices = new List<tblDevices>();
            List<DevicesViewModel> objDeviceViewModel = new List<DevicesViewModel>();
            DeviceGroupViewModel objDeviceGroupViewModel = new DeviceGroupViewModel();
            MonitoringManager objMonitoringManager = new MonitoringManager();

            objDeviceGroupViewModel = objAccessPointManager.GetAccessPointGroupsId(Id);

            objAccessPointGroupViewModel.GroupId = objDeviceGroupViewModel.GroupId;

            objAccessPointGroupViewModel.GroupName = objDeviceGroupViewModel.GroupName;

            if (objDeviceGroupViewModel.AccessPointGroupViewModel.SelectedDevices != null && objDeviceGroupViewModel.AccessPointGroupViewModel.SelectedDevices.Any())
            {
                objAccessPointGroupViewModel.SelectedDevices = objDeviceGroupViewModel.AccessPointGroupViewModel.SelectedDevices;
            }

            objOtherDevices = objMonitoringManager.GetDeviceByTypeId((int)DeviceTypes.AccessPoints);

            objOtherDevices.ForEach(x => { objDeviceViewModel.Add(new DevicesViewModel() { DeviceId = x.DeviceId, DeviceName = x.DeviceName }); });

            objAccessPointGroupViewModel.Devices = objDeviceViewModel;

            return objAccessPointGroupViewModel;
        }
    }
}
