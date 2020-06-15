using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using SM.Managers;
using SM.Models;
using SM.Models.Table;
using SM.Service.Controller;
using SM.Service.Models;

namespace SM.Service
{
    static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [DllImport("kernel32", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main(String[] args)
        {
            if (Environment.UserInteractive)
            {
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
                    if (args.Any<String>(x => x == "-u" || x == "--uninstall"))
                    {
                        if (Helper.ServiceInstaller.Uninstall("SM.Service"))
                            Console.WriteLine("Dienst wurde erfolgreich deinstalliert...");
                        else
                            Console.WriteLine("Dienst konnte nicht deinstalliert werden!");
                    }
                }
            }
            else
            {
                File.AppendAllText("log.txt", "ServicesToRun");
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new ServiceManager() };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
