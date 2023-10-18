using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MonitoringLibrary;
using Monitoring.Common;
using Monitoring.Common.Logging;
using System.Net;

namespace MonitoringService
{
    public partial class MonitoringService : ServiceBase
    {

        List<ServerMonitoringModel> objLstServerMonitoringModel = new List<ServerMonitoringModel>();
        TimerIntervalsViewModel objTimerIntervalsViewModel = new TimerIntervalsViewModel();

        public System.Timers.Timer heartBeatTimer = new System.Timers.Timer();
        public System.Timers.Timer updateMonitoringTimer = new System.Timers.Timer();
        public System.Timers.Timer remoteUpdateMonitoringTimer = new System.Timers.Timer();

        public MonitoringService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                GetTimerIntervals().Wait();

                RegisterDevice().Wait();

                heartBeatTimer.Interval = objTimerIntervalsViewModel.HeartBeatInterval.Value;
                updateMonitoringTimer.Interval = objTimerIntervalsViewModel.MonitoringTimerInterval.Value;
                remoteUpdateMonitoringTimer.Interval = objTimerIntervalsViewModel.MonitoringJsonUploadTimerInterval.Value;

                heartBeatTimer.Elapsed += new System.Timers.ElapsedEventHandler(heartBeatTimer_Elapsed);
                updateMonitoringTimer.Elapsed += new System.Timers.ElapsedEventHandler(updateMonitoringTimer_Elapsed);
                remoteUpdateMonitoringTimer.Elapsed += new System.Timers.ElapsedEventHandler(remoteUpdateMonitoringTimer_Elapsed);

                heartBeatTimer.Start();
                updateMonitoringTimer.Start();
                remoteUpdateMonitoringTimer.Start();
            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }
        }

        protected override void OnStop()
        {
            try
            {
                heartBeatTimer.Stop();
                updateMonitoringTimer.Stop();
            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }
        }

        private void UpdateheartBeat()
        {
            try
            {
                MonitoringManager objMonitoringManager = new MonitoringManager();
                objMonitoringManager.UpdateheartBeat();
                objLstServerMonitoringModel.Add(objMonitoringManager.DoMonitoring());
            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }
        }

        private async Task GetTimerIntervals()
        {
            try
            {
                MonitoringManager objMonitoringManager = new MonitoringManager();
                objTimerIntervalsViewModel =await objMonitoringManager.GetTimerIntervals();
            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }
        }

        private async Task RegisterDevice()
        {
            try
            {
                MonitoringManager objMonitoringManager = new MonitoringManager();
                await objMonitoringManager.RegisterDevice();
            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }
        }

        private void heartBeatTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                UpdateheartBeat();
            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }
        }

        private void updateMonitoringTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                MonitoringManager objMonitoringManager = new MonitoringManager();
                objMonitoringManager.UpdateMonitoringDetails(objLstServerMonitoringModel);
                objLstServerMonitoringModel = new List<ServerMonitoringModel>();
            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }
        }

        private void remoteUpdateMonitoringTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                MonitoringManager objMonitoringManager = new MonitoringManager();
                objMonitoringManager.UpdateJsonToServer();
                
            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }
        }
    }
}
