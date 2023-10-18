using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;


namespace DataRecoveryServiceSetupHelper
{
    [RunInstaller(true)]
    public partial class DataRecoveryServiceInstaller : System.Configuration.Install.Installer
    {
        public DataRecoveryServiceInstaller()
        {
            InitializeComponent();
        }
    }
}
