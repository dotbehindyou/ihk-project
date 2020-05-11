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
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SM.Service
{
    public partial class ServiceManager : ServiceBase {
        private readonly ApiController apiC = new ApiController();
        private Thread serviceHanlderThread = null;

        public CancellationTokenSource CancellationTokenSource { get; private set; } = new CancellationTokenSource();
        public CancellationToken CancellationToken { get => CancellationTokenSource.Token; }

        public ServiceManager()
        {
            InitializeComponent();

            Config.Current = new Config() { ConnectionString = ConfigurationManager.AppSettings["connectionString"] };
        }

        protected override void OnStart(string[] args)
        {
            Config.Current = new Config() { ConnectionString = ConfigurationManager.AppSettings["connectionString"] };

            serviceHanlderThread = new Thread(this.ServiceHandler);
        }

        protected override void OnStop()
        {
            if(serviceHanlderThread?.Join(3000) ?? false)
                serviceHanlderThread.Abort();
        }

        private void ServiceHandler()
        {
            Thread moduleSyncThread, statusSyncThread;
            ManualResetEvent moduleSyncThreadResetEvent = new ManualResetEvent(false),
                statusSyncThreadResetEvent = new ManualResetEvent(false);

            List<SM_Modules_Installed> installedModules = null;
            using (CustomerServiceManager sm = new CustomerServiceManager())
                installedModules = sm.GetMany();

            moduleSyncThread = new Thread(() =>
            {
                while (!CancellationToken.IsCancellationRequested)
                {
                    this.ModuleSync(installedModules);
                    Thread.Sleep(1800 * 1000); // Jede hable Stunde die Version / Validation_Token abrufen
                }

                moduleSyncThreadResetEvent.Set();
            });

            statusSyncThread = new Thread(() =>
            {
                while (!CancellationToken.IsCancellationRequested)
                {
                    this.StatusSync(installedModules);
                    Thread.Sleep(60 * 1000); // Jede Minute den Status aktualisieren 
                }

                statusSyncThreadResetEvent.Set();
            });

            moduleSyncThread.Start();
            statusSyncThread.Start();

            WaitHandle[] waitHandles = new WaitHandle[]
            {
                moduleSyncThreadResetEvent,
                statusSyncThreadResetEvent
            };

            WaitHandle.WaitAll(waitHandles);
        }

        private void ModuleSync(List<SM_Modules_Installed> installedModules)
        {
            ModuleController mc = new ModuleController();

            List<Module> modules = apiC.GetModules();

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


            foreach (var mod in result)
            {
                Models.Service serv;
                if (mod.Module.Status == "DEL")
                {
                    serv = mc.Remove(mod.Module);
                    apiC.SendRemove(serv);

                    continue;
                }
                else if (mod.Module.Status != "INIT" && mod.Module.Status != "UPDATE")
                    continue;

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

        private void StatusSync(List<SM_Modules_Installed> installedModules)
        {
            foreach (var ins in installedModules)
            {
                try
                {
                    ServiceController sc = new ServiceController(ins.ServiceName);

                    var ser = new Models.Service()
                    {
                        Status = sc.Status,
                        Name = ins.ServiceName,
                        Module = new Module
                        {
                            Module_ID = ins.Module_ID,
                            Name = ins.ModuleName,
                            Status = sc.Status.ToString(),
                            Version = sc.ServiceName
                        }
                    };

                    apiC.SendStatus(ser);
                }
                catch (Exception e)
                {

                }
            }
        }
    }
}
