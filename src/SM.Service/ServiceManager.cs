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
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace SM.Service
{
    public partial class ServiceManager : ServiceBase
    {
#if DEBUG
        private Timer updateStatus = new Timer(10 * 1000); // 10 Sekunden
        private Timer updateRessource = new Timer(30 * 1000); // 30 Sekunden

#else
        private Timer updateStatus = new Timer(300 * 1000); // 300 Sekunden
        private Timer updateRessource = new Timer(30 * 1000 * 60); // 30 min
#endif

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
            System.Net.ServicePointManager.ServerCertificateValidationCallback += delegate (
                object sender,
                X509Certificate cert,
                X509Chain chain,
                SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            updateStatus.Elapsed += this.OnUpdateStatus;
            updateRessource.Elapsed += this.OnUpdateRessources;

            logWriter = new StreamWriter(path: $"{DateTime.Now.ToString("yy-MM")}.log", append: true);
            logWriter.AutoFlush = true;
            Console.SetOut(logWriter);

            Console.WriteLine("ServiceManager initialisiert");

            Config.Current = new Config() { ConnectionString = ConfigurationManager.AppSettings["connectionString"] };

            serviceHanlderThread = new Thread(this.ServiceHandler);
            serviceHanlderThread.Start();
        }

        protected override void OnStop()
        {
            CancellationTokenSource.Cancel();

            if ((serviceHanlderThread?.IsAlive ?? false) && (serviceHanlderThread?.Join(3000) ?? false))
                serviceHanlderThread.Abort();

            logWriter?.Dispose();

            Console.WriteLine("ServiceManager wurde gestoppt");
        }

        private void ServiceHandler()
        {
            Thread.Sleep(10000);
            Console.WriteLine($"[{DateTime.Now}] ServiceHandler wird initialisiert");
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
                    try
                    {
                        Console.WriteLine($"[{DateTime.Now}] Module werden syncronisiert...");
                        this.ModuleSync(installedModules);
                    }
                    catch (Exception e)
                    {

                    }
                    Thread.Sleep(1800 * 1000); // Jede hable Stunde die Version / Validation_Token abrufen
                }

                moduleSyncThreadResetEvent.Set();
            });

            statusSyncThread = new Thread(() =>
            {
                while (!CancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        Console.WriteLine($"[{DateTime.Now}] Status wird aktualisiert...");
                        this.StatusSync(installedModules);
                    }
                    catch (Exception e)
                    {

                    }
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

            try
            {
                WaitHandle.WaitAll(waitHandles);
            }
            catch (ThreadAbortException e)
            {

            }
        }

        private void ModuleSync(List<SM_Modules_Installed> installedModules)
        {
            ModuleController mc = new ModuleController();
            List<Module> modules = null;
            try
            {
                modules = apiC.GetModules();
            }
            catch (Exception e)
            {

            }

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
                    Console.WriteLine($"[{DateTime.Now}] Module '{mod.Module.Name}' wird gelöscht.");

                    serv = mc.Remove(mod.Module);
                    apiC.SendRemove(serv);

                    continue;
                }
                else if (mod.Module.Status != "INIT" && mod.Module.Status != "UPDATE")
                    continue;

                if (mod.IsInstalled)
                {
                    Console.WriteLine($"[{DateTime.Now}] Module '{mod.Module.Name}' wird aktualisiert.");

                    Byte[] file = null;
                    SM_Modules_Installed sm = installedModules.Where(x => x.Module_ID == mod.Module.Module_ID).FirstOrDefault();

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
                    Console.WriteLine($"[{DateTime.Now}] Module '{mod.Module.Name}' wird instaliert.");

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
