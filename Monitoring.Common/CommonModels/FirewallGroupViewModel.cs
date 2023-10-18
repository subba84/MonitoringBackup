using Monitoring.Common.CommonModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Common.CommonModels
{
    public class FirewallGroupViewModel
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int Interval { get; set; }
        public int Retries { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public List<DevicesViewModel> Devices { get; set; }
        public List<string> SelectedDevices { get; set; }
    }
}
