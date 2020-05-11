using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using SM.Managers;
using SM.Models;
using SM.Models.Table;
using SM.Service.Controller;
using SM.Service.Models;

namespace SM.Service
{
    static class Program {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main(String[] args)
        {

            if (Environment.UserInteractive)
            {
                Config.Current = new Config() { ConnectionString = ConfigurationManager.AppSettings["connectionString"] };

                Models.Service SM_Service = new Models.Service
                {
                    Name = "ServiceManager_Weiss",
                    Path = Assembly.GetExecutingAssembly().Location,
                };

                if (args?.Any(x => x.ToLower() == "-i") ?? false) // -i als Parameter zum installieren
                {
                    Helper.ServiceInstaller.Install(SM_Service.Path, SM_Service.Name, "Service Manager");
                }
                else if (args?.Any(x => x.ToLower() == "-u") ?? false) // -u als Parameter zum deinstallieren
                {
                    Helper.ServiceInstaller.Uninstall(SM_Service.Name);
                }

                if (args?.Any(x => x.ToLower() == "-d") ?? false) // -d zum Debuggen 
                {
                    // TODO Debuggen ServiceBase.Run(new ServiceManager());
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new ServiceManager() };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
