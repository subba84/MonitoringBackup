using Monitoring.Common.CommonModels;
using MonitoringWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.DataAccess.DAL
{
    public class PrinterManager
    {
        public List<tblDeviceGroups> GetPrinterGroups()
        {
            using (var context = new MonitoringContext())
            {
                return context.tblDeviceGroups.Where(k => k.IsActive == true && k.DeviceTypeId == (int)DeviceTypes.Printers).ToList();
            }
        }

        public void CreatePrintersGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            using (var context = new MonitoringContext())
            {
                tblDeviceGroups objtblDeviceGroups = new tblDeviceGroups() { GroupName = objDeviceGroupViewModel.GroupName, DeviceTypeId = objDeviceGroupViewModel.DeviceTypeId, IsActive = true, CreatedBy = "System", CreatedOn = DateTime.Now };
                context.tblDeviceGroups.Add(objtblDeviceGroups);
                context.SaveChanges();

                if (objDeviceGroupViewModel.DeviceTypeId == (int)DeviceTypes.Printers)
                {
                    tblPrinterGroups objtblPrinterGroups = new tblPrinterGroups();
                    objtblPrinterGroups.GroupId = objtblDeviceGroups.GroupId;
                    objtblPrinterGroups.CreatedBy = "System";
                    objtblPrinterGroups.CreatedOn = DateTime.Now;
                    objtblPrinterGroups.Interval = objDeviceGroupViewModel.PrinterGroupViewModel.Interval;
                    objtblPrinterGroups.IsActive = true;
                    objtblPrinterGroups.Retries = objDeviceGroupViewModel.PrinterGroupViewModel.Retries;

                    context.tblPrinterGroups.Add(objtblPrinterGroups);
                    context.SaveChanges();
                }
            }
        }

        public void UpdatePrintersGroups(DeviceGroupViewModel objDeviceGroupViewModel)
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

                    var existingRecord = context.tblPrinterGroups.Where(k => k.GroupId == objDeviceGroupViewModel.GroupId).FirstOrDefault();
                    existingRecord.Interval = objDeviceGroupViewModel.PrinterGroupViewModel.Interval;
                    existingRecord.Retries = objDeviceGroupViewModel.PrinterGroupViewModel.Retries;
                    existingRecord.UpdatedBy = "System";
                    existingRecord.UpdatedOn = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public void DeletePrintersGroups(int groupId)
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

                if (existingRecord.DeviceTypeId == (int)DeviceTypes.Printers)
                {
                    var existingthresholds = context.tblPrinterGroups.Where(k => k.GroupId == groupId).FirstOrDefault();
                    existingthresholds.IsActive = false;
                    existingRecord.UpdatedBy = "System";
                    existingRecord.UpdatedOn = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public DeviceGroupViewModel GetPrintersGroupsId(int Id)
        {
            DeviceGroupViewModel objDeviceGroupViewModel = new DeviceGroupViewModel();
            PrinterGroupViewModel objPrinterGroupViewModel = new PrinterGroupViewModel();

            using (var context = new MonitoringContext())
            {
                var groups = context.tblDeviceGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objDeviceGroupViewModel.GroupId = groups.GroupId;
                objDeviceGroupViewModel.DeviceTypeId = groups.DeviceTypeId;
                objDeviceGroupViewModel.GroupName = groups.GroupName;

                var objtblPrinterGroups = context.tblPrinterGroups.Where(k => k.IsActive == true && k.GroupId == Id).FirstOrDefault();
                objPrinterGroupViewModel.Id = objtblPrinterGroups.Id;
                objPrinterGroupViewModel.Interval = objtblPrinterGroups.Interval.HasValue? objtblPrinterGroups.Interval.Value:0;
                objPrinterGroupViewModel.Retries = objtblPrinterGroups.Retries.HasValue? objtblPrinterGroups.Retries.Value:0;
                if (!string.IsNullOrEmpty(objtblPrinterGroups.Devices))
                {
                    objPrinterGroupViewModel.SelectedDevices = objtblPrinterGroups.Devices.Split(',').ToList();
                }

                objDeviceGroupViewModel.PrinterGroupViewModel = objPrinterGroupViewModel;

            }

            return objDeviceGroupViewModel;

        }

        public void MapPrinterDevicetoGroup(tblPrinterGroups objtblPrinterGroups)
        {
            using (var context = new MonitoringContext())
            {
                if (context.tblDeviceGroups.Where(k => k.GroupId == objtblPrinterGroups.GroupId).Any())
                {
                    var existingrecord = context.tblPrinterGroups.Where(k => k.GroupId == objtblPrinterGroups.GroupId).FirstOrDefault();
                    existingrecord.Devices = objtblPrinterGroups.Devices;
                    context.SaveChanges();
                }
            }
        }

        public List<PingDeviceDetails> GetPrinterDetails()
        {
            List<PingDeviceDetails> objDevices = new List<PingDeviceDetails>();

            using (var context = new MonitoringContext())
            {
                objDevices = (from p in context.tblDevices.Where(k => k.IsActive == true)
                              join q in context.tblDeviceTypes.Where(k => k.CanPingable.Value == true && k.IsActive == true)
                                on p.DeviceTypeId equals q.AssetTypeId
                              from r in context.tblPrinterGroups.Where(k => k.IsActive == true && k.Devices.Contains(p.DeviceName))
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
