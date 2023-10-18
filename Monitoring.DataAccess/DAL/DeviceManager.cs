using Monitoring.Common;
using MonitoringWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.DataAccess.DAL
{
    public class DeviceManager
    {
        public void AddDevice(tblDevices objtblDevices)
        {
            using (var context = new MonitoringContext())
            {
                context.tblDevices.Add(objtblDevices);
                context.SaveChanges();
            }
        }

        public void UpdateDevice(tblDevices objtblDevices)
        {
            using (var context = new MonitoringContext())
            {
                var existingRecord = context.tblDevices.Where(k => k.DeviceId == objtblDevices.DeviceId).FirstOrDefault();

                if (existingRecord != null)
                {
                    existingRecord.CategoryId = objtblDevices.CategoryId;
                    existingRecord.DeviceName = objtblDevices.DeviceName;
                    existingRecord.DeviceTypeId = objtblDevices.DeviceTypeId;
                    existingRecord.DisplayName = objtblDevices.DisplayName;
                    existingRecord.IpAddress = objtblDevices.IpAddress;
                    existingRecord.Location = objtblDevices.Location;
                    existingRecord.ManagedTypeId = objtblDevices.ManagedTypeId;
                    existingRecord.Password = objtblDevices.Password;
                    existingRecord.Status = objtblDevices.Status;
                    existingRecord.SubCategoryId = objtblDevices.SubCategoryId;
                    existingRecord.UpdatedBy = objtblDevices.UpdatedBy;
                    existingRecord.UpdatedOn = objtblDevices.UpdatedOn;
                    existingRecord.UserId = objtblDevices.UserId;
                    existingRecord.VendorId = objtblDevices.VendorId;

                    context.SaveChanges();
                }
            }
        }

        public void DeleteDevice(int Id)
        {
            using (var context = new MonitoringContext())
            {
                var existingRecord = context.tblDevices.Where(k => k.DeviceId == Id).FirstOrDefault();

                if (existingRecord != null)
                {
                    existingRecord.IsActive = false;
                    context.SaveChanges();
                }
            }
        }

        public List<tblDevices> GetDevices()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDevices.Where(k => k.IsActive == true).ToList();
            }
        }

        public tblDevices GetDeviceById(int Id)
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDevices.Where(k => k.IsActive == true && k.DeviceId==Id).FirstOrDefault();
            }
        }

        public List<tblDevices> GetDeviceByTypeId(int Id)
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDevices.Where(k => k.IsActive == true && k.DeviceTypeId == Id).ToList();
            }
        }

        public tblDevices GetDeviceByName(string deviceName)
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDevices.Where(k => k.IsActive == true && k.DeviceName == deviceName).FirstOrDefault();
            }
        }

        public List<tblDeviceTypes> GetDeviceTypes()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDeviceTypes.Where(k => k.IsActive == true).ToList();
            }
        }

    }
}
