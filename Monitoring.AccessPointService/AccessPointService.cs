using Monitoring.Common;
using Monitoring.Common.Logging;
using Monitoring.PingLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.PingService
{
    public partial class AccessPointService : ServiceBase
    {
        public System.Timers.Timer pingTimer = new System.Timers.Timer();

        public AccessPointService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                pingTimer.AutoReset = false;
                pingTimer.Elapsed += new System.Timers.ElapsedEventHandler(pingTimer_Elapsed);
                pingTimer.Start();

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
               
            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }

        }

        private void pingTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                AccessPointPingManager objPingManager = new AccessPointPingManager();
                objPingManager.DoPinging();
            }
            catch (Exception ex)
            {
                DetailsLogger.LogInfo(ex.Message + ex.StackTrace);
            }
        }


    }
}
