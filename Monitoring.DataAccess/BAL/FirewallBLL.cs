using Monitoring.Common.CommonModels;
using Monitoring.DataAccess.DAL;
using MonitoringWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.DataAccess.BAL
{
    public class FirewallBLL
    {
        FirewallManager objFirewallManager = new FirewallManager();

        public List<tblDeviceGroups> GetFirewallGroups()
        {
            return objFirewallManager.GetFirewallGroups();
        }

        public void CreateFirewallGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            objFirewallManager.CreateFirewallGroups(objDeviceGroupViewModel);
        }

        public void UpdateFirewallGroups(DeviceGroupViewModel objDeviceGroupViewModel)
        {
            objFirewallManager.UpdateFirewallGroups(objDeviceGroupViewModel);
        }

        public void DeleteFirewallGroups(int groupId)
        {
            objFirewallManager.DeleteFirewallGroups(groupId);
        }

        public DeviceGroupViewModel GetFirewallGroupsId(int Id)
        {
            return objFirewallManager.GetFirewallGroupsId(Id);
        }

        public void MapFireWallDevicetoGroup(FirewallGroupViewModel objFirewallGroupViewModel)
        {
            tblFirewallGroups objtblFirewallGroups = new tblFirewallGroups();
            objtblFirewallGroups.GroupId = objFirewallGroupViewModel.GroupId;
            objtblFirewallGroups.Devices = string.Join(",", objFirewallGroupViewModel.SelectedDevices);


            objFirewallManager.MapFireWallDevicetoGroup(objtblFirewallGroups);
        }

        public List<PingDeviceDetails> GetFirewallDetails()
        {
            return objFirewallManager.GetFirewallDetails();
        }
    }
}
