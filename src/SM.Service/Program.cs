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

                Config.Current = new Config() { ConnectionString = ConfigurationManager.AppSettings["connectionString"] };

                ApiController apiC = new ApiController();

                List<Module> modules = apiC.GetModules();

                List<SM_Modules_Installed> installedModules = null;
                using (CustomerServiceManager sm = new CustomerServiceManager())
                    installedModules = sm.GetMany();

                //var t = modules.Join(installedModules, i => i.Module_ID, m => m.Module_ID, (m, i) => new { m, i });
                var result = from mod in modules
                             join ins in installedModules on mod.Module_ID equals ins.Module_ID into jmods
                             from jmod in jmods.DefaultIfEmpty()
                             select new
                             {
                                 Module = mod,
                                 ServiceInformation = jmod,
                                 IsInstalled = jmod != null
                             };

                ModuleController mc = new ModuleController();

                foreach(var mod in result)
                {
                    Models.Service serv;
                    if (mod.Module.Status == "DEL")
                    {
                        serv = mc.Remove(mod.Module);
                        apiC.SendRemove(serv);

                        continue;
                    }
                    
                    if (mod.IsInstalled)
                    {
                        Byte[] file = null;
                        SM_Modules_Installed sm = installedModules.Where(x=> x.Module_ID == mod.Module.Module_ID).FirstOrDefault();

                        if (sm.ValidationToken != mod.Module.Validation_Token)
                        {
                            file = apiC.GetFile(mod.Module);
                        }

                        try
                        {
                            serv = mc.Set(mod.Module, file);
                            apiC.SendStatus(serv);
                            continue;
                        }
                        catch (ServiceNotInstalledException e)
                        {

                        }
                    }

                    try
                    {
                        Byte[] file = apiC.GetFile(mod.Module);
                        serv = mc.Add(mod.Module, file);
                        apiC.SendStatus(serv);
                    }
                    catch (ServiceAlreadyInstalledException e)
                    {

                    }
                }
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
