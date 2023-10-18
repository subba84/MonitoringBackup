using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Common
{
    public static class Constants
    {
        public static string CreatedBy = "System";
        //public static string GetGroupsUrl = ConfigurationManager.AppSettings["GetServerGroupsUrl"].ToString();
        //public static string GetSlidesUrl = ConfigurationManager.AppSettings["GetSlidesUrl"].ToString();

    }

    public enum DeviceStatus
    {
        Working =1,
        NotWorking =2
    }
}
