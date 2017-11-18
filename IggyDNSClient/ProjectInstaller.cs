using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace IggyDNSClient
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
            eventLog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("IggyDNSClientInstaller"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "IggyDNSClientInstaller", "Application");
            }
            eventLog.Source = "IggyDNSClientInstaller";
            eventLog.Log = "Application";
        }

        private void ServiceInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            eventLog.WriteEntry("IggyDNSService Installer - Starting service");
            using (ServiceController sc = new ServiceController(this.ServiceInstaller.ServiceName))
            {
                sc.Start();
            }
        }

        private void ServiceInstaller_BeforeUninstall(object sender, InstallEventArgs e)
        {
            eventLog.WriteEntry("IggyDNSService Installer - Stopping service");
            using (ServiceController sc = new ServiceController(this.ServiceInstaller.ServiceName))
            {
                sc.Stop();
            }
        }

        private void ServiceProcessInstaller_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
