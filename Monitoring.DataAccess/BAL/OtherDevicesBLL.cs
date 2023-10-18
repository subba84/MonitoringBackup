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
    public class OtherDevicesBLL
    {
        OtherDeviceManager otherDeviceManager = new OtherDeviceManager();

        public List<GroupViewModel> GetOtherDeviceGroups()
        {
            List<tblDeviceGroups> objlsttblServerGroups = new List<tblDeviceGroups>();
            List<GroupViewModel> objlstGroupViewModel = new List<GroupViewModel>();
            objlsttblServerGroups = otherDeviceManager.GetOtherDeviceGroups();

            objlsttblServerGroups.ForEach(x => { objlstGroupViewModel.Add(new GroupViewModel() { GroupId = x.GroupId, DeviceTypeName = ((DeviceTypes)x.DeviceTypeId).ToString(), GroupName = x.GroupName }); });

            return objlstGroupViewModel;
        }

        public DeviceGroupViewModel GetOtherDeviceGroupsId(int Id)
        {
            return otherDeviceManager.GetOtherDeviceGroupsId(Id);
        }

        public void CreateGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            otherDeviceManager.CreateOtherDeviceGroups(objDeviceGroupViewModel);
        }

        public void updateGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            otherDeviceManager.UpdateOtherDeviceGroups(objDeviceGroupViewModel);
        }

        public void removeGroups(int Id)
        {
            otherDeviceManager.DeleteOtherDeviceGroups(Id);
        }

        public void MapOtherDevicetoGroup(OtherDeviceGroupViewModel objOtherDeviceGroupViewModel)
        {
            tblOtherDeviceGroups objtblServerGroups = new tblOtherDeviceGroups();
            objtblServerGroups.GroupId = objOtherDeviceGroupViewModel.GroupId;
            objtblServerGroups.Devices = string.Join(",", objOtherDeviceGroupViewModel.SelectedDevices);

            otherDeviceManager.MapOtherDevicetoGroup(objtblServerGroups);
        }

        public OtherDeviceGroupViewModel GetOtherDeviceGroupDetailsById(int Id)
        {
            OtherDeviceGroupViewModel objOtherDeviceGroupViewModel = new OtherDeviceGroupViewModel();

            List<tblDevices> objOtherDevices = new List<tblDevices>();
            List<DevicesViewModel> objDeviceViewModel = new List<DevicesViewModel>();
            DeviceGroupViewModel objDeviceGroupViewModel = new DeviceGroupViewModel();
            MonitoringManager objMonitoringManager = new MonitoringManager();

            objDeviceGroupViewModel = otherDeviceManager.GetOtherDeviceGroupsId(Id);

            objOtherDeviceGroupViewModel.GroupId = objDeviceGroupViewModel.GroupId;

            objOtherDeviceGroupViewModel.GroupName = objDeviceGroupViewModel.GroupName;

            objOtherDeviceGroupViewModel.SelectedDevices = objDeviceGroupViewModel.otherDeviceGroupViewModel.SelectedDevices;

            objOtherDevices = objMonitoringManager.GetDeviceByTypeId((int)DeviceTypes.OtherDevices);

            objOtherDevices.ForEach(x => { objDeviceViewModel.Add(new DevicesViewModel() { DeviceId = x.DeviceId, DeviceName = x.DeviceName }); });

            objOtherDeviceGroupViewModel.Devices = objDeviceViewModel;

            return objOtherDeviceGroupViewModel;
        }

        public List<PingDeviceDetails> GetOtherDeviceDetails()
        {
            return otherDeviceManager.GetOtherDeviceDetails();
        }
    }
}
