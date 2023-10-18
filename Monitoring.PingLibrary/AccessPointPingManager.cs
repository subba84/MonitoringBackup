using Monitoring.Common;
using Monitoring.Common.CommonModels;
using Monitoring.Common.CommonModels.ViewModels;
using Monitoring.Common.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Monitoring.PingLibrary
{
    public class AccessPointPingManager
    {
        string GetAccessPointDetailsUrl, UpdateDeviceStatusUrl, GetAppSettingsUrl = string.Empty;
        AppSettingsVideModel objAppSettingsVideModel = new AppSettingsVideModel();
        List<PingDeviceDetails> objPingDeviceDetails = new List<PingDeviceDetails>();
        string deviceFolderName = "AccessPoints";

        public AccessPointPingManager()
        {
            HttpManager objhttpManager = new HttpManager();

            string baseUrl = HttpManager.GetWebServiceBaseUrl();

            GetAccessPointDetailsUrl = baseUrl + ConfigurationManager.AppSettings["GetAccessPointDetailsUrl"].ToString();

            UpdateDeviceStatusUrl = baseUrl + ConfigurationManager.AppSettings["UpdateDeviceStatusUrl"].ToString();

            GetAppSettingsUrl = baseUrl + ConfigurationManager.AppSettings["GetAppSettingsUrl"].ToString();
        }

        public async Task GetAppSettings()
        {
            HttpManager objhttpManager = new HttpManager();
            objAppSettingsVideModel = await objhttpManager.GetRequest<AppSettingsVideModel>(GetAppSettingsUrl);
        }

        public async Task UpdateDeviceStatus(int deviceId, int status)
        {
            try
            {
                HttpManager objhttpManager = new HttpManager();
                await objhttpManager.GetRequest<object>(string.Format(UpdateDeviceStatusUrl + "?deviceId={0}&status={1}", deviceId, status));
            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }
            
        }

        public async Task GetPingDevicesDetails()
        {
            HttpManager objhttpManager = new HttpManager();
            objPingDeviceDetails = await objhttpManager.GetRequest<List<PingDeviceDetails>>(GetAccessPointDetailsUrl);
        }

        public void DoPinging()
        {
            try
            {
                GetAppSettings().Wait();
                GetPingDevicesDetails().Wait();

                foreach (var item in objPingDeviceDetails)
                {
                    try
                    {
                        var t = new Thread(async () => await PingDevice(item.DeviceId, item.DeviceName, item.Interval, item.Retries, objAppSettingsVideModel.PingDevicesLogLocation, item.IPAddress));
                        t.Start();
                    }
                    catch (Exception ex)
                    {
                        DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
                    }
                   
                }
            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }
        }


        public async Task PingDevice(int deviceId, string deviceName, int Interval, int retries, string logFolderPath,string ipAddesss)
        {
            try
            {
                PingDeviceStatus objPingDeviceStatus;
                DetailsLogger objDetailsLogger = new DetailsLogger();
                bool result = false;
                int deviceStatus = 0;
                Ping ping = new Ping();
                for (int i = 0; i < retries; i++)
                {
                    PingReply pingreply = ping.Send(ipAddesss);
                    if (pingreply.Status == IPStatus.Success)
                    {
                        result = true;
                        objPingDeviceStatus = new PingDeviceStatus();
                        objPingDeviceStatus.DeviceName = deviceName;
                        objPingDeviceStatus.Result = result;
                        objPingDeviceStatus.TimeStamp = DateTime.Now;
                        deviceStatus = (int)DeviceStatus.Working;
                        objDetailsLogger.UpdateDeviceDetails(objPingDeviceStatus, logFolderPath, deviceFolderName);
                        await UpdateDeviceStatus(deviceId, deviceStatus);
                        break;
                    }
                    else
                    {
                        result = false;
                        deviceStatus = (int)DeviceStatus.NotWorking;
                        objPingDeviceStatus = new PingDeviceStatus();
                        objPingDeviceStatus.DeviceName = deviceName;
                        objPingDeviceStatus.Result = result;
                        objPingDeviceStatus.TimeStamp = DateTime.Now;
                        objDetailsLogger.UpdateDeviceDetails(objPingDeviceStatus, logFolderPath, deviceFolderName);
                    }
                }

                await UpdateDeviceStatus(deviceId, deviceStatus);
            }
            catch (PingException ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }
        }
    }
}
