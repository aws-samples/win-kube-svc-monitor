using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MonitorService
{
    public partial class KubeSvcMonitor : ServiceBase
    {
        private Timer timer;
        private readonly string SERVICE_NAME = "KubeSvcMonitor";
        public KubeSvcMonitor()
        {
            InitializeComponent();
            eventLog1 = new EventLog();
            if (!EventLog.SourceExists(SERVICE_NAME))
            {
                EventLog.CreateEventSource(
                    SERVICE_NAME, SERVICE_NAME);
            }
            eventLog1.Source = SERVICE_NAME;
            eventLog1.Log = SERVICE_NAME;
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry($"{SERVICE_NAME} starting", EventLogEntryType.Information);
            timer = new Timer();
            timer.Interval = Int32.Parse(ConfigurationManager.AppSettings["timerInterval"]) * 1000;
            timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
            timer.Start();
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            var installedServices = ServiceController.GetServices();
            var servicesToMonitor = ConfigurationManager.AppSettings["services"].Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var service in servicesToMonitor)
            {
                try
                {
                    var found = installedServices.Where(installed => installed.ServiceName.ToUpperInvariant() == service.ToUpperInvariant() || installed.DisplayName.ToUpperInvariant() == service.ToUpperInvariant()).FirstOrDefault();
                    if (found == null)
                    {
                        eventLog1.WriteEntry($"Service {service} not found", EventLogEntryType.Error);
                        continue;
                    }
                    eventLog1.WriteEntry($"Service {service} found. Status = {found.Status}", EventLogEntryType.Information);
                    if (found.Status != ServiceControllerStatus.Running)
                    {
                        found.Start();
                        found.Refresh();
                        eventLog1.WriteEntry($"Service {service} restarted. Status = {found.Status}", EventLogEntryType.Information);
                    }
                }
                catch (Exception ex)
                {
                    eventLog1.WriteEntry(ex.ToString(), EventLogEntryType.Error);
                }
            }
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry($"{SERVICE_NAME} stopping", EventLogEntryType.Information);
            timer?.Stop();
        }
    }
}
