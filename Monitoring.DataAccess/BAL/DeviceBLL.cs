using Monitoring.Common;
using Monitoring.Common.CommonModels.ViewModels;
using Monitoring.DataAccess.DAL;
using MonitoringWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.DataAccess.BAL
{
    public class DeviceBLL
    {
        public void AddDevice(DevicesViewModel objDevicesViewModel)
        {
            DeviceManager objDeviceManager = new DeviceManager();
            tblDevices objtblDevices = new tblDevices();
            objtblDevices.CategoryId = objDevicesViewModel.CategoryId;
            objtblDevices.CreatedBy = Constants.CreatedBy;
            objtblDevices.CreatedOn = DateTime.Now;
            objtblDevices.DeviceName = objDevicesViewModel.DeviceName;
            objtblDevices.DeviceTypeId = objDevicesViewModel.DeviceTypeId;
            objtblDevices.DisplayName = objDevicesViewModel.DisplayName;
            objtblDevices.IpAddress = objDevicesViewModel.IpAddress;
            objtblDevices.IsActive = true;
            objtblDevices.Location = objDevicesViewModel.Location;
            objtblDevices.ManagedTypeId = objDevicesViewModel.ManagedTypeId;
            objtblDevices.Password = objDevicesViewModel.Password;
            objtblDevices.Status = objDevicesViewModel.Status;
            objtblDevices.SubCategoryId = objDevicesViewModel.SubCategoryId;
            objtblDevices.UserId = objDevicesViewModel.UserId;
            objtblDevices.VendorId = objDevicesViewModel.VendorId;

            objDeviceManager.AddDevice(objtblDevices);
        }

        public void UpdateDevice(DevicesViewModel objDevicesViewModel)
        {
            DeviceManager objDeviceManager = new DeviceManager();
            tblDevices objtblDevices = new tblDevices();
            objtblDevices.DeviceId = objDevicesViewModel.DeviceId;
            objtblDevices.CategoryId = objDevicesViewModel.CategoryId;
            objtblDevices.CreatedBy = Constants.CreatedBy;
            objtblDevices.CreatedOn = DateTime.Now;
            objtblDevices.DeviceName = objDevicesViewModel.DeviceName;
            objtblDevices.DeviceTypeId = objDevicesViewModel.DeviceTypeId;
            objtblDevices.DisplayName = objDevicesViewModel.DisplayName;
            objtblDevices.IpAddress = objDevicesViewModel.IpAddress;
            objtblDevices.IsActive = true;
            objtblDevices.Location = objDevicesViewModel.Location;
            objtblDevices.ManagedTypeId = objDevicesViewModel.ManagedTypeId;
            objtblDevices.Password = objDevicesViewModel.Password;
            objtblDevices.Status = objDevicesViewModel.Status;
            objtblDevices.SubCategoryId = objDevicesViewModel.SubCategoryId;
            objtblDevices.UserId = objDevicesViewModel.UserId;
            objtblDevices.VendorId = objDevicesViewModel.VendorId;

            objDeviceManager.UpdateDevice(objtblDevices);
        }

        public void DeleteDevice(int Id)
        {
            DeviceManager objDeviceManager = new DeviceManager();
            objDeviceManager.DeleteDevice(Id);
        }

        public List<DevicesViewModel> GetDevices()
        {
            List<DevicesViewModel> result = new List<DevicesViewModel>();
            DeviceManager objDeviceManager = new DeviceManager();
            var devices = objDeviceManager.GetDevices();
            devices.ForEach(x => { result.Add(new DevicesViewModel { CategoryId = x.CategoryId, CreatedBy = x.CreatedBy, CreatedOn = x.CreatedOn, DeviceId = x.DeviceId, DeviceName = x.DeviceName, DeviceTypeId = x.DeviceTypeId, DisplayName = x.DisplayName, IpAddress = x.IpAddress, IsActive = x.IsActive, Location = x.Location, ManagedTypeId = x.ManagedTypeId.HasValue ? x.ManagedTypeId.Value : 0, Password = x.Password, Status = x.Status, SubCategoryId = x.SubCategoryId, UserId = x.UserId, VendorId = x.VendorId.HasValue ? x.VendorId.Value : 0 }); });

            return result;
        }

        public DevicesViewModel GetDeviceById(int Id)
        {
            DevicesViewModel objDevicesViewModel = new DevicesViewModel();
            DeviceManager objDeviceManager = new DeviceManager();
            var device = objDeviceManager.GetDeviceById(Id);

            objDevicesViewModel.CategoryId = device.CategoryId;
            objDevicesViewModel.CreatedBy = device.CreatedBy;
            objDevicesViewModel.CreatedOn = device.CreatedOn;
            objDevicesViewModel.DeviceId = device.DeviceId;
            objDevicesViewModel.DeviceName = device.DeviceName;
            objDevicesViewModel.DeviceTypeId = device.DeviceTypeId;
            objDevicesViewModel.DisplayName = device.DisplayName;
            objDevicesViewModel.IpAddress = device.IpAddress;
            objDevicesViewModel.IsActive = device.IsActive;
            objDevicesViewModel.Location = device.Location;
            objDevicesViewModel.ManagedTypeId = device.ManagedTypeId.HasValue ? device.ManagedTypeId.Value : 0;
            objDevicesViewModel.Password = device.Password;
            objDevicesViewModel.Status = device.Status;
            objDevicesViewModel.SubCategoryId = device.SubCategoryId;
            objDevicesViewModel.UserId = device.UserId;
            objDevicesViewModel.VendorId = device.VendorId.HasValue ? device.VendorId.Value : 0;

            return objDevicesViewModel;
        }

        public DevicesViewModel GetDeviceByName(string deviceName)
        {
            DevicesViewModel objDevicesViewModel = new DevicesViewModel();
            DeviceManager objDeviceManager = new DeviceManager();
            var device = objDeviceManager.GetDeviceByName(deviceName);

            objDevicesViewModel.CategoryId = device.CategoryId;
            objDevicesViewModel.CreatedBy = device.CreatedBy;
            objDevicesViewModel.CreatedOn = device.CreatedOn;
            objDevicesViewModel.DeviceId = device.DeviceId;
            objDevicesViewModel.DeviceName = device.DeviceName;
            objDevicesViewModel.DeviceTypeId = device.DeviceTypeId;
            objDevicesViewModel.DisplayName = device.DisplayName;
            objDevicesViewModel.IpAddress = device.IpAddress;
            objDevicesViewModel.IsActive = device.IsActive;
            objDevicesViewModel.Location = device.Location;
            objDevicesViewModel.ManagedTypeId = device.ManagedTypeId.HasValue ? device.ManagedTypeId.Value : 0;
            objDevicesViewModel.Password = device.Password;
            objDevicesViewModel.Status = device.Status;
            objDevicesViewModel.SubCategoryId = device.SubCategoryId;
            objDevicesViewModel.UserId = device.UserId;
            objDevicesViewModel.VendorId = device.VendorId.HasValue ? device.VendorId.Value : 0;

            return objDevicesViewModel;
        }

        public List<DevicesViewModel> GetDeviceByTypeById(int Id)
        {
            List<DevicesViewModel> result = new List<DevicesViewModel>();
            DeviceManager objDeviceManager = new DeviceManager();
            var devices = objDeviceManager.GetDeviceByTypeId(Id);


            devices.ForEach(x => { result.Add(new DevicesViewModel { CategoryId = x.CategoryId, CreatedBy = x.CreatedBy, CreatedOn = x.CreatedOn, DeviceId = x.DeviceId, DeviceName = x.DeviceName, DeviceTypeId = x.DeviceTypeId, DisplayName = x.DisplayName, IpAddress = x.IpAddress, IsActive = x.IsActive, Location = x.Location, ManagedTypeId = x.ManagedTypeId.HasValue ? x.ManagedTypeId.Value : 0, Password = x.Password, Status = x.Status, SubCategoryId = x.SubCategoryId, UserId = x.UserId, VendorId = x.VendorId.HasValue ? x.VendorId.Value : 0 }); });

            return result;
        }

        public List<DeviceTypesViewModel> GetDeviceTypes()
        {
            List<DeviceTypesViewModel> objDeviceTypesViewModel = new List<DeviceTypesViewModel>();
            DeviceManager objDeviceManager = new DeviceManager();
            var deviceTypes = objDeviceManager.GetDeviceTypes();
            deviceTypes.ForEach(x => { objDeviceTypesViewModel.Add(new DeviceTypesViewModel { CanPingable =x.CanPingable.HasValue ? x.CanPingable.Value : false, DeviceTypeId =x.AssetTypeId, DeviceTypeName =x.AssetTypeName});   });

            return objDeviceTypesViewModel;
        }
    }
}
