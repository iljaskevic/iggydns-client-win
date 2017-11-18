using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace IggyDNSClient
{
    public partial class IggyDNSClient : ServiceBase
    {
        private static string API_KEY;
        public IggyDNSClient()
        {
            InitializeComponent();
            eventLog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("IggyDNSClient"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "IggyDNSClient", "Application");
            }
            eventLog.Source = "IggyDNSClient";
            eventLog.Log = "Application";
        }

        protected override void OnStart(string[] args)
        {
            eventLog.WriteEntry("IggyDNSService - OnStart called");
            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\IggyDNS\IggyDNSClient", false))
                {
                    API_KEY = (String)key.GetValue("APIKeyValue");
                    eventLog.WriteEntry($"IggyDNSService - key - {API_KEY}");
                }
            } catch (Exception e)
            {
                eventLog.WriteEntry($"IggyDNSService - key error - {e.Message}");
            }
        }

        protected override void OnStop()
        {
        }

        private void Timer_Tick(object sender, ElapsedEventArgs e)
        {
            eventLog.WriteEntry("IggyDNSService - timer1_Tick called");
            using (var client = new HttpClient())
            {
                var m = new HttpRequestMessage(HttpMethod.Head, "https://requestb.in/1cf8zv61");
                m.Headers.Add("x-api-key", API_KEY);
                
                var resp = client.SendAsync(m).Result;
                eventLog.WriteEntry($"IggyDNSService - timer1_Tick - request response {resp.StatusCode}");
            }
        }
    }
}
