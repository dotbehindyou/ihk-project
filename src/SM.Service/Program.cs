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

                ModuleController mc = new ModuleController();

                for(int i = 0; i < modules.Count; ++i)
                {
                    var m = modules[i];
                    try
                    {
                        if (m.Status == "INIT")
                        {
                            Byte[] file = apiC.GetFile(m);
                            apiC.SendStatus(mc.Add(m, file));
                        }
                        else if (m.Status == "UPDATE")
                        {
                            Byte[] file = null;
                            SM_Modules_Installed sm = installedModules.Where(x=> x.Module_ID == m.Module_ID).FirstOrDefault();
                            if(sm == null || sm.ValidationToken == null)
                            {
                                m.Status = "INIT";
                                --i;
                                continue;
                            }

                            if (sm.ValidationToken != m.Validation_Token)
                            {
                                file = apiC.GetFile(m);
                            }
                            apiC.SendStatus(mc.Set(m, file));
                        }
                        else if (m.Status == "DEL")
                        {
                            apiC.SendRemove(mc.Remove(m));
                        }
                    }
                    catch(ServiceNotInstalledException e)
                    {
                        if(m.Status != "DEL")
                        {
                            m.Status = "INIT";
                            --i;
                        }
                    }
                    catch(ServiceAlreadyInstalledException e)
                    {
                        m.Status = "UPDATE";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
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
