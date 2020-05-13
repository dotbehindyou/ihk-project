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
                using (StreamWriter logWriter = new StreamWriter(path: $"{DateTime.Now:yy-MM}.install.log", append: true))
                {
                    logWriter.AutoFlush = true;
                    Console.SetOut(logWriter);

                    Console.WriteLine("ServiceManager");

                    Config.Current = new Config() { ConnectionString = ConfigurationManager.AppSettings["connectionString"] };

                    Models.Service SM_Service = new Models.Service
                    {
                        Name = "ServiceManager_Weiss",
                        Path = Assembly.GetExecutingAssembly().Location,
                    };

                    if (args?.Any(x => x.ToLower() == "-i") ?? false) // -i als Parameter zum installieren
                    {
                        if (Helper.ServiceInstaller.Install(SM_Service.Path, SM_Service.Name, "Service Manager"))
                            Console.WriteLine("Service Manager installiert!");
                        else
                            Console.WriteLine("Service Manager konnte nicht installiert werden!");
                    }
                    else if (args?.Any(x => x.ToLower() == "-u") ?? false) // -u als Parameter zum deinstallieren
                    {
                        if(Helper.ServiceInstaller.Uninstall(SM_Service.Name))
                            Console.WriteLine("Service Manager entfernt!");
                        else
                            Console.WriteLine("Service Manager konnte nicht deinstalliert werden!");
                    }

                    if (args?.Any(x => x.ToLower() == "-d") ?? false) // -d zum Debuggen
                    {
                        // TODO Debuggen ServiceBase.Run(new ServiceManager());
                    }
                }
            }
            else
            {
                Console.WriteLine($"[{DateTime.Now}] Init Dienst");
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new ServiceManager() };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
