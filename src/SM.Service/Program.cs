using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        static void Main()
        {
            if (Environment.UserInteractive)
            {
                // TODO Install

            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new ServiceManager()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
