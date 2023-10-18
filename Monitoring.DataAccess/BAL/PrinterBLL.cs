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
    public class PrinterBLL
    {
        PrinterManager objPrinterManager = new PrinterManager();

        public List<GroupViewModel> GetPrinterGroups()
        {
            List<tblDeviceGroups> objlsttblServerGroups = new List<tblDeviceGroups>();
            List<GroupViewModel> objlstGroupViewModel = new List<GroupViewModel>();
            objlsttblServerGroups = objPrinterManager.GetPrinterGroups();

            objlsttblServerGroups.ForEach(x => { objlstGroupViewModel.Add(new GroupViewModel() { GroupId = x.GroupId, DeviceTypeName = ((DeviceTypes)x.DeviceTypeId).ToString(), GroupName = x.GroupName }); });

            return objlstGroupViewModel;
        }

        public void CreatePrintersGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            objPrinterManager.CreatePrintersGroups(objDeviceGroupViewModel);
        }

        public void UpdatePrintersGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            objPrinterManager.UpdatePrintersGroups(objDeviceGroupViewModel);
        }

        public void DeletePrintersGroups(int groupId)
        {
            objPrinterManager.DeletePrintersGroups(groupId);
        }

        public DeviceGroupViewModel GetPrintersGroupsId(int Id)
        {
            return objPrinterManager.GetPrintersGroupsId(Id);

        }

        public void MapPrinterDevicetoGroup(PrinterGroupViewModel objPrinterGroupViewModel)
        {
            tblPrinterGroups objtblPrinterGroups = new tblPrinterGroups();
            objtblPrinterGroups.GroupId = objPrinterGroupViewModel.GroupId;
            objtblPrinterGroups.Devices = string.Join(",", objPrinterGroupViewModel.SelectedDevices);
            objPrinterManager.MapPrinterDevicetoGroup(objtblPrinterGroups);
        }

        public List<PingDeviceDetails> GetPrinterDetails()
        {
            return objPrinterManager.GetPrinterDetails();
        }

        public PrinterGroupViewModel GetPrinterGroupDetailsById(int Id)
        {
            PrinterGroupViewModel objPrinterGroupViewModel = new PrinterGroupViewModel();

            List<tblDevices> objOtherDevices = new List<tblDevices>();
            List<DevicesViewModel> objDeviceViewModel = new List<DevicesViewModel>();
            DeviceGroupViewModel objDeviceGroupViewModel = new DeviceGroupViewModel();
            MonitoringManager objMonitoringManager = new MonitoringManager();

            objDeviceGroupViewModel = objPrinterManager.GetPrintersGroupsId(Id);

            objPrinterGroupViewModel.GroupId = objDeviceGroupViewModel.GroupId;

            objPrinterGroupViewModel.GroupName = objDeviceGroupViewModel.GroupName;

            if (objDeviceGroupViewModel.PrinterGroupViewModel.SelectedDevices != null && objDeviceGroupViewModel.PrinterGroupViewModel.SelectedDevices.Any())
            {
                objPrinterGroupViewModel.SelectedDevices = objDeviceGroupViewModel.PrinterGroupViewModel.SelectedDevices;
            }

            objOtherDevices = objMonitoringManager.GetDeviceByTypeId((int)DeviceTypes.Printers);

            objOtherDevices.ForEach(x => { objDeviceViewModel.Add(new DevicesViewModel() { DeviceId = x.DeviceId, DeviceName = x.DeviceName }); });

            objPrinterGroupViewModel.Devices = objDeviceViewModel;

            return objPrinterGroupViewModel;
        }
    }
}
