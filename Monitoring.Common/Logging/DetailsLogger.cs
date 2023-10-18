using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using ObjectsComparer;
using Monitoring.Common.CommonModels.ViewModels;

namespace Monitoring.Common.Logging
{
    public class DetailsLogger
    {
        public void UpdateServerMonirotingDetails(List<ServerMonitoringModel> ObjmonitoringModel, string logFolderPath)
        {
            List<ServerMonitoringModel> objlstServerMonitoringModel = new List<ServerMonitoringModel>();

            string serverFolderPath = Path.Combine(logFolderPath, "ServerLogs");
            if (!Directory.Exists(serverFolderPath))
            {
                Directory.CreateDirectory(serverFolderPath);
            }

            string serverLogFile = ObjmonitoringModel.First().ServerName + "_log.json";
            string serverLogFilePath = Path.Combine(serverFolderPath, serverLogFile);
            if (File.Exists(serverLogFilePath))
            {
                objlstServerMonitoringModel = JsonConvert.DeserializeObject<List<ServerMonitoringModel>>(File.ReadAllText(serverLogFilePath));
            }

            objlstServerMonitoringModel.AddRange(ObjmonitoringModel);

            string updatedJson = JsonConvert.SerializeObject(objlstServerMonitoringModel);

            File.WriteAllText(serverLogFilePath, updatedJson);

        }

        public void UpdateDeviceDetails(PingDeviceStatus objPingDeviceStatus, string logFolderPath, string deviceFolderPath)
        {
            List<PingDeviceStatus> objlstPingDeviceModel = new List<PingDeviceStatus>();

            string pingDeviceFolderPath = Path.Combine(logFolderPath, deviceFolderPath);
            if (!Directory.Exists(pingDeviceFolderPath))
            {
                Directory.CreateDirectory(pingDeviceFolderPath);
            }

            string pingDeviceLogFile = objPingDeviceStatus.DeviceName + "_log.json";

            string pingDeviceLogFilePath = Path.Combine(pingDeviceFolderPath, pingDeviceLogFile);

            if (File.Exists(pingDeviceLogFilePath))
            {
                objlstPingDeviceModel = JsonConvert.DeserializeObject<List<PingDeviceStatus>>(File.ReadAllText(pingDeviceLogFilePath));
            }

            objlstPingDeviceModel.Add(objPingDeviceStatus);

            string updatedJson = JsonConvert.SerializeObject(objlstPingDeviceModel);

            File.WriteAllText(pingDeviceLogFilePath, updatedJson);

        }

        public static void LogInfo(string message)
        {
            string filename = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + "EventLog.txt";
            using (StreamWriter w = File.AppendText(filename))
            {
                w.WriteLine(string.Format("Date : {0} --- Event : {1} ", DateTime.Now.ToString(), message));
            }
        }

        public void UpdateServerMonirotingDetailstoServer(string logFolderPath, string ServerLogFolderPath)
        {
            List<ServerMonitoringModel> currentData = new List<ServerMonitoringModel>();
            List<ServerMonitoringModel> remoteData = new List<ServerMonitoringModel>();

            string ServerFolderPath = Path.Combine(ServerLogFolderPath, "ServerLogs");

            if (!Directory.Exists(ServerFolderPath))
            {
                Directory.CreateDirectory(ServerFolderPath);
            }

            string ServerFolderFilePath = Path.Combine(ServerFolderPath, Environment.MachineName + "_log.json");

            string LogFolderPath = Path.Combine(logFolderPath, "ServerLogs");

            string LogFolderFilePath = Path.Combine(LogFolderPath, Environment.MachineName + "_log.json");

            if (File.Exists(LogFolderFilePath))
            {
                currentData = JsonConvert.DeserializeObject<List<ServerMonitoringModel>>(File.ReadAllText(LogFolderFilePath));

                if (File.Exists(ServerFolderFilePath))
                {
                    remoteData = JsonConvert.DeserializeObject<List<ServerMonitoringModel>>(File.ReadAllText(ServerFolderFilePath));

                    //IEnumerable<Difference> differences;
                    //var comparer = new ObjectsComparer.Comparer<List<ServerMonitoringModel>>();

                    //var isEqual = comparer.Compare(currentData, remoteData, out differences);

                    //var newData = currentData.Except(remoteData);

                    //foreach (var item in remoteData)
                    //{
                    //    currentData.Remove(item);
                    //}

                    string updatedJson = JsonConvert.SerializeObject(currentData);
                    File.WriteAllText(ServerFolderFilePath, updatedJson);

                }

                else
                {
                    string updatedJson = JsonConvert.SerializeObject(currentData);
                    File.WriteAllText(ServerFolderFilePath, updatedJson);
                }
            }

        }
    }
}
