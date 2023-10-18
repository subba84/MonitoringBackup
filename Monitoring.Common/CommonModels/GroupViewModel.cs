using Monitoring.Common.CommonModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Common.CommonModels
{
    public class DeviceGroupViewModel
    {
        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public int DeviceTypeId { get; set; }

        public ServerGroupViewModel ServerGroupViewModel { get; set; }

        public OtherDeviceGroupViewModel otherDeviceGroupViewModel { get; set; }

        public AccessPointGroupViewModel AccessPointGroupViewModel { get; set; }

        public CctvCameraGroupViewModel CctvCameraGroupViewModel { get; set; }

        public FirewallGroupViewModel FirewallGroupViewModel { get; set; }

        public NetworkSwitchGroupViewModel NetworkSwitchGroupViewModel { get; set; }

        public PrinterGroupViewModel PrinterGroupViewModel { get; set; }
    }

    public class ServerGroupViewModel
    {
        public int Id { get; set; }

        public int GroupId { get; set; }

        public string GroupName { get; set; }

        public List<DevicesViewModel> Servers { get; set; }

        public List<string> SelectedServers { get; set; }

        public int CpuWarning { get; set; }

        public int CpuCritical { get; set; }

        public int MemoryWarning { get; set; }

        public int MemoryCritical { get; set; }

        public int NetworkWarning { get; set; }

        public int NetworkCritical { get; set; }

        public int DiskWarning { get; set; }

        public int DiskCritical { get; set; }
    }

}
