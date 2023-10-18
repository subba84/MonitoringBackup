using System;

namespace Monitoring.Common
{
    public class ServerMonitoringModel
    {
        public string ServerName { get; set; }

        public DateTime? LastDiscoveredTime { get; set; }

        public CpuDetails CpuDetails { get; set; }

        public MemoryDetails MemoryDetails { get; set; }

        public NetworkDetails NetworkDetails { get; set; }

        public DiskDetails DiskDetails { get; set; }

    }

    public  class CpuDetails
    {
        public int CpuUtilization { get; set; }

        public int Processes { get; set; }

        public int Threads { get; set; }

        public int Handles { get; set; }
    }

    public class MemoryDetails
    {
        public int MemoryUtilization { get; set; }

        public int MemoryUsed { get; set; }

        public int MemoryInTotal { get; set; }

        public int MemoryAvailable { get; set; }
    }

    public class NetworkDetails
    {
        public int Sent { get; set; }

        public int Received { get; set; }
    }

    public class DiskDetails
    {
        public int DiskInUse { get; set; }

        public int DiskInTotal { get; set; }

        public int DiskAvailable { get; set; }

        public int DiskUtilization { get; set; }
    }


}
