using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Common.CommonModels
{
    public class PingDeviceDetails
    {
        public int DeviceId { get; set; }

        public string DeviceName { get; set; }

        public string IPAddress { get; set; }

        public int Retries { get; set; }

        public int Interval { get; set; }

        public string LogLocation { get; set; }
    }
}
