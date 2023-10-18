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
    public class CctvCameraBLL
    {
        CctvCameraManager objCctvCameraManager = new CctvCameraManager();
        public List<GroupViewModel> GetCCTVCameraGroups()
        {
            List<tblDeviceGroups> objlsttblServerGroups = new List<tblDeviceGroups>();
            List<GroupViewModel> objlstGroupViewModel = new List<GroupViewModel>();
            objlsttblServerGroups = objCctvCameraManager.GetCCTVCameraGroups();

            objlsttblServerGroups.ForEach(x => { objlstGroupViewModel.Add(new GroupViewModel() { GroupId = x.GroupId, DeviceTypeName = ((DeviceTypes)x.DeviceTypeId).ToString(), GroupName = x.GroupName }); });

            return objlstGroupViewModel;
        }

        public void CreateCctvCameraGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            objCctvCameraManager.CreateCctvCameraGroups(objDeviceGroupViewModel);
        }

        public void UpdateCctvCameraGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            objCctvCameraManager.UpdateCctvCameraGroups(objDeviceGroupViewModel);
        }

        public void DeleteCctvCameraGroups(int groupId)
        {
            objCctvCameraManager.DeleteCctvCameraGroups(groupId);
        }

        public DeviceGroupViewModel GetCctvCameraGroupsId(int Id)
        {
            return objCctvCameraManager.GetCctvCameraGroupsId(Id);

        }

        public void MapCctvCamerastoGroup(CctvCameraGroupViewModel objCctvCameraGroupViewModel)
        {
            tblCctvCameraGroups objtblCctvCameraGroups = new tblCctvCameraGroups();
            objtblCctvCameraGroups.GroupId = objCctvCameraGroupViewModel.GroupId;
            objtblCctvCameraGroups.Devices = string.Join(",", objCctvCameraGroupViewModel.SelectedDevices);

            objCctvCameraManager.MapCctvCamerastoGroup(objtblCctvCameraGroups);
        }

        public List<PingDeviceDetails> GetCctvCameraDetails()
        {
            return objCctvCameraManager.GetCctvCameraDetails();
        }

        public CctvCameraGroupViewModel GetCCtvCameraGroupDetailsById(int Id)
        {
            CctvCameraGroupViewModel objCctvCameraGroupViewModel = new CctvCameraGroupViewModel();

            List<tblDevices> objOtherDevices = new List<tblDevices>();
            List<DevicesViewModel> objDeviceViewModel = new List<DevicesViewModel>();
            DeviceGroupViewModel objDeviceGroupViewModel = new DeviceGroupViewModel();
            MonitoringManager objMonitoringManager = new MonitoringManager();

            objDeviceGroupViewModel = objCctvCameraManager.GetCctvCameraGroupsId(Id);

            objCctvCameraGroupViewModel.GroupId = objDeviceGroupViewModel.GroupId;

            objCctvCameraGroupViewModel.GroupName = objDeviceGroupViewModel.GroupName;

            if (objDeviceGroupViewModel.CctvCameraGroupViewModel.SelectedDevices != null && objDeviceGroupViewModel.CctvCameraGroupViewModel.SelectedDevices.Any())
            {
                objCctvCameraGroupViewModel.SelectedDevices = objDeviceGroupViewModel.CctvCameraGroupViewModel.SelectedDevices;
            }

            objOtherDevices = objMonitoringManager.GetDeviceByTypeId((int)DeviceTypes.CCTVCameras);

            objOtherDevices.ForEach(x => { objDeviceViewModel.Add(new DevicesViewModel() { DeviceId = x.DeviceId, DeviceName = x.DeviceName }); });

            objCctvCameraGroupViewModel.Devices = objDeviceViewModel;

            return objCctvCameraGroupViewModel;
        }
    }
}
