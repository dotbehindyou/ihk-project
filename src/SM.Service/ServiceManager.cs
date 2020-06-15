using SM.Managers;
using SM.Models;
using SM.Models.Table;
using SM.Service.Controller;
using SM.Service.Models;
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

namespace SM.Service
{
    public partial class ServiceManager : ServiceBase
    {
        private Timer updateStatus = new Timer(300 * 1000);
        private Timer updateRessource = new Timer(30 * 1000 * 60);

        private ApiController apiC = new ApiController();

        public ServiceManager()
        {
            InitializeComponent();

            Config.Current = new Config() { ConnectionString = ConfigurationManager.AppSettings["connectionString"] };
        }

        protected override void OnStart(string[] args)
        {
            updateStatus.Elapsed += this.OnUpdateStatus;
            updateRessource.Elapsed += this.OnUpdateRessources;

            updateStatus.AutoReset = true;
            updateRessource.AutoReset = true;

            updateStatus.Start();
            updateRessource.Start();
        }

        protected override void OnPause()
        {
            updateStatus.Stop();
            updateRessource.Stop();

            base.OnPause();
        }

        protected override void OnContinue()
        {
            updateStatus.Start();
            updateRessource.Start();

            base.OnContinue();
        }

        private void OnUpdateStatus(Object sender, ElapsedEventArgs eventArgss)
        {
            updateStatus.Stop();
            try
            {
                List<SM_Modules_Installed> installedModules = null;
                using (CustomerServiceManager sm = new CustomerServiceManager())
                    installedModules = sm.GetMany();

                ModuleController mc = new ModuleController();

                foreach (var mo in installedModules)
                {
                    apiC.SendStatus(mc.GetService(mo));
                }

            }
            catch(Exception e)
            {

            }
            updateStatus.Start();
        }
        private void OnUpdateRessources(Object sender, ElapsedEventArgs eventArgs)
        {
            updateRessource.Stop();
            try
            {
                List<Module> modules = apiC.GetModules();

                List<SM_Modules_Installed> installedModules = null;
                using (CustomerServiceManager sm = new CustomerServiceManager())
                    installedModules = sm.GetMany();

                ModuleController mc = new ModuleController();

                for (int i = 0; i < modules.Count; ++i)
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
                    catch (ServiceNotInstalledException e)
                    {
                        if (m.Status != "DEL")
                        {
                            m.Status = "INIT";
                            --i;
                        }
                    }
                    catch (ServiceAlreadyInstalledException e)
                    {
                        m.Status = "UPDATE";
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
            catch (Exception e)
            {

            }
            updateRessource.Start();
        }

        protected override void OnStop()
        {
        }
    }
}
