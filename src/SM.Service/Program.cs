using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
<<<<<<< Updated upstream
=======
using System.Runtime.InteropServices;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

>>>>>>> Stashed changes
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main(String[] args)
        {
            if (Environment.UserInteractive)
            {
<<<<<<< Updated upstream
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
=======
                File.AppendAllText("log_.txt", "UserInteractive");
                int u;
                IntPtr ptr = GetForegroundWindow();
                GetWindowThreadProcessId(ptr, out u);
                Process process = Process.GetProcessById(u);
                if (!AttachConsole(process.Id)) AllocConsole();

                if (args.Length < 1 || args.Any<String>(x => x == "-i" || x == "--install"))
                {
                    String path = Assembly.GetEntryAssembly().Location;
                    if (Helper.ServiceInstaller.Install(path, "SM.Service", "Service Manager"))
                        Console.WriteLine("Dienst wurde installiert...");
                    else
                        Console.WriteLine("Dienste konnte nicht installiert werden!");
                }
                else
                {
                    if(args.Any<String>(x=> x == "-u" || x == "--uninstall"))
                    {
                        if (Helper.ServiceInstaller.Uninstall("SM.Service"))
                            Console.WriteLine("Dienst wurde erfolgreich deinstalliert...");
                        else
                            Console.WriteLine("Dienst konnte nicht deinstalliert werden!");
>>>>>>> Stashed changes
                    }
                }
            }
            else
            {
<<<<<<< Updated upstream
                Console.WriteLine($"[{DateTime.Now}] Init Dienst");
=======
                File.AppendAllText("log.txt", "ServicesToRun");
>>>>>>> Stashed changes
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new ServiceManager() };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
