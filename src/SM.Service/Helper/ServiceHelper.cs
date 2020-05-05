using System;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Collections;
using System.IO;
using System.Linq;
using SM.Service.Models;

namespace SM.Service.Helper
{
    internal static class ServiceHelper
    {
        public static ServiceController Install(Models.Service service, params String[] args)
        {
            String pathExe = Path.Combine(service.Path, service.Module.Name + ".exe");

            if (!File.Exists(pathExe))
                throw new FileNotFoundException();

            try
            {
                if (!ServiceInstaller.Install(pathExe, service.Name, service.Module.Name))
                    throw new ServiceException(service, "Could not installed!");

                var sc = new ServiceController(service.Name);

                if (sc.Status != ServiceControllerStatus.Running)
                    sc.Start();

                return sc;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public static void Remove(Models.Service service, params String[] args)
        {
            String pathExe = Path.Combine(service.Path, service.Module.Name + ".exe");

            try
            {
                var sc = new ServiceController(service.Name);
                if (sc.Status != ServiceControllerStatus.Stopped)
                    sc.Stop();

                if(!ServiceInstaller.Uninstall(service.Name))
                    throw new ServiceException(service, "Could not uninstalled!");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Boolean Exist(Models.Service service)
        {
            return ServiceController.GetServices().Any(x => x.ServiceName == service.Name);
        }

        public static ServiceController GetServiceController(Models.Service service)
        {
            return new ServiceController(service.Name);
        }

        public static void Start(Models.Service service)
        {
            GetServiceController(service).Start();
        }

        public static void Stop(Models.Service service)
        {
            GetServiceController(service).Stop();
        }

        public static ServiceControllerStatus Status(Models.Service service)
        {
            return GetServiceController(service).Status;
        }
    }
}
