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
    public class NetworkSwitchBLL
    {
        NetworkSwitchManager objNetworkSwitchManager = new NetworkSwitchManager();

        public List<GroupViewModel> GetNetworkSwitchGroups()
        {
            List<tblDeviceGroups> objlsttblServerGroups = new List<tblDeviceGroups>();
            List<GroupViewModel> objlstGroupViewModel = new List<GroupViewModel>();
            objlsttblServerGroups = objNetworkSwitchManager.GetNetworkSwitchGroups();

            objlsttblServerGroups.ForEach(x => { objlstGroupViewModel.Add(new GroupViewModel() { GroupId = x.GroupId, DeviceTypeName = ((DeviceTypes)x.DeviceTypeId).ToString(), GroupName = x.GroupName }); });

            return objlstGroupViewModel;
        }

        public void CreateNetworkSwitchGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            objNetworkSwitchManager.CreateNetworkSwitchGroups(objDeviceGroupViewModel);
        }

        public void UpdateNetworkSwitchGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            objNetworkSwitchManager.UpdateNetworkSwitchGroups(objDeviceGroupViewModel);
        }

        public void DeleteNetworkSwitchGroups(int groupId)
        {
            objNetworkSwitchManager.DeleteNetworkSwitchGroups(groupId);
        }

        public DeviceGroupViewModel GetNetworkSwitchGroupsId(int Id)
        {
            return objNetworkSwitchManager.GetNetworkSwitchGroupsId(Id);

        }

        public void MapNetworkSwitchGroups(NetworkSwitchGroupViewModel objNetworkSwitchGroupViewModel)
        {
            tblNetworkswitchGroups objtblNetworkswitchGroups = new tblNetworkswitchGroups();
            objtblNetworkswitchGroups.GroupId = objNetworkSwitchGroupViewModel.GroupId;
            objtblNetworkswitchGroups.Devices = string.Join(",", objNetworkSwitchGroupViewModel.SelectedDevices);

            objNetworkSwitchManager.MapNetworkSwitchGroups(objtblNetworkswitchGroups);
        }

        public List<PingDeviceDetails> GetNetworkSwitchDetails()
        {
            return objNetworkSwitchManager.GetNetworkSwitchDetails();
        }

        public NetworkSwitchGroupViewModel GetNetworkSwitchGroupDetailsById(int Id)
        {
            NetworkSwitchGroupViewModel objNetworkSwitchGroupViewModel = new NetworkSwitchGroupViewModel();

            List<tblDevices> objOtherDevices = new List<tblDevices>();
            List<DevicesViewModel> objDeviceViewModel = new List<DevicesViewModel>();
            DeviceGroupViewModel objDeviceGroupViewModel = new DeviceGroupViewModel();
            MonitoringManager objMonitoringManager = new MonitoringManager();

            objDeviceGroupViewModel = objNetworkSwitchManager.GetNetworkSwitchGroupsId(Id);

            objNetworkSwitchGroupViewModel.GroupId = objDeviceGroupViewModel.GroupId;

            objNetworkSwitchGroupViewModel.GroupName = objDeviceGroupViewModel.GroupName;

            if (objDeviceGroupViewModel.NetworkSwitchGroupViewModel.SelectedDevices != null && objDeviceGroupViewModel.NetworkSwitchGroupViewModel.SelectedDevices.Any())
            {
                objNetworkSwitchGroupViewModel.SelectedDevices = objDeviceGroupViewModel.NetworkSwitchGroupViewModel.SelectedDevices;
            }

            objOtherDevices = objMonitoringManager.GetDeviceByTypeId((int)DeviceTypes.NetworkSwitches);

            objOtherDevices.ForEach(x => { objDeviceViewModel.Add(new DevicesViewModel() { DeviceId = x.DeviceId, DeviceName = x.DeviceName }); });

            objNetworkSwitchGroupViewModel.Devices = objDeviceViewModel;

            return objNetworkSwitchGroupViewModel;
        }
    }
}
