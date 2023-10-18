using Monitoring.Common;
using Monitoring.PingLibrary;
using MonitoringLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            PingManager objPingManager = new PingManager();
            objPingManager.DoPinging();

            //AccessPointPingManager objPingManager = new AccessPointPingManager();
            //objPingManager.DoPinging();
        }
    }
}
