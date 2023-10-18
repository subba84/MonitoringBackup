using Monitoring.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management.Automation;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using System.Configuration;
using Monitoring.Common.Logging;
using Monitoring.Common.CommonModels;
using Monitoring.Common.CommonModels.ViewModels;

namespace MonitoringLibrary
{
    public class MonitoringManager
    {
        string registerDeviceUrl, updateHeartBeatUrl, updateMonitoringDetailsUrl, getTimerIntervalUrl = string.Empty, getAppSettingsUrl = string.Empty;
        HttpManager objHttpManager = new HttpManager();

        public MonitoringManager()
        {
            HttpManager objhttpManager = new HttpManager();
            string baseUrl = HttpManager.GetWebServiceBaseUrl();
            registerDeviceUrl = baseUrl + ConfigurationManager.AppSettings["RegisterDeviceUrl"].ToString();
            updateHeartBeatUrl = baseUrl + ConfigurationManager.AppSettings["UpdateHeartBeatUrl"].ToString();
            updateMonitoringDetailsUrl = baseUrl + ConfigurationManager.AppSettings["UpdateMonitoringDetails"].ToString();
            getTimerIntervalUrl = baseUrl + ConfigurationManager.AppSettings["GetTimerIntervalUrl"].ToString();
            getAppSettingsUrl = baseUrl + ConfigurationManager.AppSettings["GetAppSettingsUrl"].ToString();
        }

        public void UpdateheartBeat()
        {
            ServerMonitoringModel objServerMonitoringModel = new ServerMonitoringModel();
            objServerMonitoringModel = DoMonitoring();
            objServerMonitoringModel.LastDiscoveredTime = DateTime.Now;
            objHttpManager.PostRequest<ServerMonitoringModel>(updateHeartBeatUrl, objServerMonitoringModel).Wait();
        }

        public async Task<TimerIntervalsViewModel> GetTimerIntervals()
        {
            TimerIntervalsViewModel objTimerIntervalsViewModel =await  objHttpManager.GetRequest<TimerIntervalsViewModel>(getTimerIntervalUrl);
            return objTimerIntervalsViewModel;
        }

        public async Task RegisterDevice()
        {
            DevicesViewModel objDevicesViewModel = new DevicesViewModel();
            objDevicesViewModel.DeviceName = Environment.MachineName;
            objDevicesViewModel.CreatedOn = DateTime.Now;
            objDevicesViewModel.CreatedBy = "System";
            objDevicesViewModel.IsActive = true;
            objDevicesViewModel.DeviceTypeId =(int)DeviceTypes.Servers;

            await objHttpManager.PostRequest<DevicesViewModel>(registerDeviceUrl, objDevicesViewModel);
        }

        public void UpdateMonitoringDetails(List<ServerMonitoringModel> objLstServerMonitoringModel)
        {
            AppSettingsVideModel objAppSettingsVideModel = objHttpManager.GetRequest<AppSettingsVideModel>(getAppSettingsUrl).Result;
            DetailsLogger objDetailsLogger = new DetailsLogger();
            objDetailsLogger.UpdateServerMonirotingDetails(objLstServerMonitoringModel, objAppSettingsVideModel.MonitoringLogLocation);
        }

        public ServerMonitoringModel DoMonitoring()
        {
            ServerMonitoringModel monitoringModel = new ServerMonitoringModel();
            try
            {
                monitoringModel.ServerName = Environment.MachineName;
                using (PowerShell ps = PowerShell.Create())
                {
                    ps.AddCommand("Set-ExecutionPolicy")
                      .AddParameter("ExecutionPolicy", "RemoteSigned")
                      .AddParameter("Scope", "CurrentUser")
                      .AddParameter("Force");

                    string script = "Set-ExecutionPolicy RemoteSigned -Scope CurrentUser" + System.Environment.NewLine;

                    string processorCmd = "$cpuPercentage = (Get-WmiObject -ComputerName " + Environment.MachineName + " -Class win32_processor -ErrorAction Stop | Measure-Object -Property LoadPercentage -Average | Select-Object Average).Average" + System.Environment.NewLine +
                        "$cpuThreads = (Get-CimInstance Win32_Processor).ThreadCount" + System.Environment.NewLine +
                        "$cpuProcesses = (Get-Process | Select-Object -Property Name).count" + System.Environment.NewLine +
                        "$cpuHandles = Get-Process | Select-Object -Property @{'Name' = 'handles'; Expression= { [int]($_.Handles) }} | Measure-Object -Property 'handles' -Sum" + System.Environment.NewLine +
                        "$cpuDetails =" + System.Environment.NewLine +
                        "@{cpuPercentage = $cpuPercentage" + System.Environment.NewLine +
                        "cpuThreads = $cpuThreads" + System.Environment.NewLine +
                        "cpuProcesses = $cpuProcesses" + System.Environment.NewLine +
                        "cpuHandles = $cpuHandles.Sum}" + System.Environment.NewLine +
                        "$cpu = New-Object -TypeName PsObject -Property $cpuDetails" + System.Environment.NewLine +
                        "$cpu" + System.Environment.NewLine;

                    script = script + processorCmd;

                    ps.AddScript(script);

                    Collection<PSObject> result = ps.Invoke();
                    CpuDetails objCpuDetails = new CpuDetails();

                    if (result != null && result.Count > 0)
                    {
                        foreach (var item in result.First().Properties)
                        {
                            //if (item.Name == "cpuProcesses")
                            //{
                            //    objCpuDetails.Processes = Convert.ToInt32(item.Value);
                            //}

                            //if (item.Name == "cpuThreads")
                            //{
                            //    objCpuDetails.Threads = Convert.ToInt32(item.Value);
                            //}

                            if (item.Name == "cpuPercentage")
                            {
                                objCpuDetails.CpuUtilization = Convert.ToInt32(item.Value);
                            }

                            //if (item.Name == "cpuHandles")
                            //{
                            //    objCpuDetails.Handles = Convert.ToInt32(item.Value);
                            //}
                        }
                    }

                    monitoringModel.CpuDetails = objCpuDetails;
                }

                using (PowerShell ps1 = PowerShell.Create())
                {
                    ps1.AddCommand("Set-ExecutionPolicy")
                      .AddParameter("ExecutionPolicy", "RemoteSigned")
                      .AddParameter("Scope", "CurrentUser")
                      .AddParameter("Force");

                    string memoryScript = "Set-ExecutionPolicy RemoteSigned -Scope CurrentUser" + System.Environment.NewLine;

                    string memoryCmd = "$ComputerMemory = Get-WmiObject -ComputerName " + Environment.MachineName + " -Class win32_operatingsystem -ErrorAction Stop" + System.Environment.NewLine +
                        "$MemoryInUse = [math]::Round(($ComputerMemory.TotalVisibleMemorySize - $ComputerMemory.FreePhysicalMemory)/(1024*1024),1)" + System.Environment.NewLine +
                        "$MemoryInTotal= [math]::Round(($ComputerMemory.TotalVisibleMemorySize)/(1024*1024),1)" + System.Environment.NewLine +
                        "$Memory = ((($ComputerMemory.TotalVisibleMemorySize - $ComputerMemory.FreePhysicalMemory)*100)/ $ComputerMemory.TotalVisibleMemorySize)" + System.Environment.NewLine +
                        "$MemoryUtilizationPercentage = [math]::Round($Memory, 2)" + System.Environment.NewLine +
                        "$memoryAvaialable= $MemoryInTotal - $MemoryInUse" + System.Environment.NewLine +
                        "$MemoryDetails = " + System.Environment.NewLine +
                        "@{MemoryInUse = $MemoryInUse" + System.Environment.NewLine +
                        "MemoryInTotal = $MemoryInTotal" + System.Environment.NewLine +
                        "MemoryUtilizationPercentage = $MemoryUtilizationPercentage" + System.Environment.NewLine +
                        "}" + System.Environment.NewLine +
                        "$Me = New-Object -TypeName PsObject -Property $MemoryDetails" + System.Environment.NewLine +
                        "$Me";

                    memoryScript = memoryScript + memoryCmd;
                    ps1.AddScript(memoryScript);
                    Collection<PSObject> result = ps1.Invoke();
                    MemoryDetails objMemoryDetails = new MemoryDetails();


                    if (result != null && result.Count > 0)
                    {
                        foreach (var item in result.First().Properties)
                        {
                            if (item.Name == "MemoryInUse")
                            {
                                objMemoryDetails.MemoryUsed = Convert.ToInt32(item.Value);
                            }
                            else if (item.Name == "MemoryInTotal")
                            {
                                objMemoryDetails.MemoryInTotal = Convert.ToInt32(item.Value);
                            }
                            else if (item.Name == "MemoryUtilizationPercentage")
                            {
                                objMemoryDetails.MemoryUtilization = Convert.ToInt32(item.Value);
                            }
                            else if (item.Name == "MemoryAvaialable")
                            {
                                objMemoryDetails.MemoryAvailable = Convert.ToInt32(item.Value);
                            }
                        }

                    }

                    monitoringModel.MemoryDetails = objMemoryDetails;

                }

                using (PowerShell ps2 = PowerShell.Create())
                {
                    ps2.AddCommand("Set-ExecutionPolicy")
                      .AddParameter("ExecutionPolicy", "RemoteSigned")
                      .AddParameter("Scope", "CurrentUser")
                      .AddParameter("Force");

                    string script = "Set-ExecutionPolicy RemoteSigned -Scope CurrentUser" + System.Environment.NewLine;

                    string diskCmd = "$freespace = Get-CimInstance -ClassName Win32_LogicalDisk | Select-Object -Property @{'Name' = 'FreeSpace (GB)'; Expression= { [int]($_.FreeSpace / 1GB) }} | Measure-Object -Property 'FreeSpace (GB)' -Sum" + System.Environment.NewLine +
                                            "$totalSpace=  Get-CimInstance -ClassName Win32_LogicalDisk | Select-Object -Property @{'Name' = 'TotalSpace (GB)'; Expression= { [int]($_.Size / 1GB) }} | Measure-Object -Property 'TotalSpace (GB)' -Sum" + System.Environment.NewLine +
                                            "$DiskPercentage =  ($freespace.sum/$totalSpace.sum)*100" + System.Environment.NewLine +
                                            "$DiskAvialable = $totalSpace.sum - $freespace.sum" + System.Environment.NewLine +
                                            "$diskDetails = " + System.Environment.NewLine +
                                            "@{DiskFreeSpace = $freespace.Sum" + System.Environment.NewLine +
                                            "DiskTotalSpace = $totalSpace.Sum" + System.Environment.NewLine +
                                            "DiskUtilizationPercentage = [math]::Round($DiskPercentage, 0)" + System.Environment.NewLine +
                                            "DiskAvialable = $DiskAvialable}" + System.Environment.NewLine +
                                            "$Disk = New-Object -TypeName PsObject -Property $diskDetails" + System.Environment.NewLine +
                                            "$Disk";

                    script = script + diskCmd;

                    ps2.AddScript(script);

                    Collection<PSObject> result = ps2.Invoke();

                    DiskDetails objDiskDetails = new DiskDetails();

                    if (result != null && result.Count > 0)
                    {
                        foreach (var item in result.First().Properties)
                        {
                            if (item.Name == "DiskFreeSpace")
                            {
                                objDiskDetails.DiskAvailable = Convert.ToInt32(item.Value);
                            }
                            else if (item.Name == "DiskTotalSpace")
                            {
                                objDiskDetails.DiskInTotal = Convert.ToInt32(item.Value);
                            }
                            else if (item.Name == "DiskTotalSpace")
                            {
                                objDiskDetails.DiskInTotal = Convert.ToInt32(item.Value);
                            }
                            else if (item.Name == "DiskAvialable")
                            {
                                objDiskDetails.DiskAvailable = Convert.ToInt32(item.Value);
                            }
                            else if (item.Name == "DiskUtilizationPercentage")
                            {
                                objDiskDetails.DiskUtilization = Convert.ToInt32(item.Value);
                            }

                        }
                    }

                    monitoringModel.DiskDetails = objDiskDetails;

                }

            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }


            return monitoringModel;
        }

        public void UpdateJsonToServer()
        {
            AppSettingsVideModel objAppSettingsVideModel = objHttpManager.GetRequest<AppSettingsVideModel>(getAppSettingsUrl).Result;
            DetailsLogger objDetailsLogger = new DetailsLogger();
            objDetailsLogger.UpdateServerMonirotingDetailstoServer(objAppSettingsVideModel.MonitoringLogLocation, objAppSettingsVideModel.MonitoringCommonLogLocation);
        }
    }
}
