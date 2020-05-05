using Ionic.Zip;
using SM.Models;
using SM.Service.Helper;
using SM.Service.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;

namespace SM.Service.Controller
{
    internal class ModuleController
    {
        private readonly String moduleServicesFile = "modules.local";

        private readonly String moduleStore;

        public ModuleController()
        {
            this.moduleStore = ConfigurationManager.AppSettings["module_store"];

            if(!Path.IsPathRooted(this.moduleStore))
                this.moduleStore = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, this.moduleStore);

            DirectoryInfo di = new DirectoryInfo(this.moduleStore);
            if (!di.Exists)
                di.Create();
        }

        #region IO

        private void WriteLocalModules(List<Models.Service> moduleServices)
        {
            using (FileStream fs = new FileStream(this.moduleServicesFile, FileMode.Create))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(fs, moduleServices);
            }
        }

        private List<Models.Service> ReadLocalModules()
        {
            using (FileStream fs = new FileStream(this.moduleServicesFile, FileMode.OpenOrCreate))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (List<Models.Service>)binaryFormatter.Deserialize(fs);
            }
        }

        #endregion

        #region ServiceManager

        public List<Models.Service> GetLocalModules()
        {
            return this.ReadLocalModules();
        }

        public Models.Service Add(Module module, Byte[] file)
        {
            String path = Path.Combine(this.moduleStore, module.Name);

            Models.Service service = new Models.Service() {
                Module = module,
                Name = module.Name,
                Path = path,
            };

            if (ServiceHelper.Exist(service))
                throw new ServiceAlreadyInstalledException(service);

            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);

            using(MemoryStream ms = new MemoryStream(file))
            using(ZipFile zf = ZipFile.Read(ms))
            {
                zf.ExtractAll(path, ExtractExistingFileAction.OverwriteSilently);
            }

            ServiceController sc = ServiceHelper.Install(service);
            service.Status = sc.Status;

            return service;
        }

        public Models.Service Set(Module module, Byte[] file = null)
        {
            String path = Path.Combine(this.moduleStore, $"{module.Name}");

            Models.Service service = new Models.Service()
            {
                Module = module,
                Name = module.Name,
                Path = path,
            };

            if (!Directory.Exists(path) || !ServiceHelper.Exist(service))
                throw new ServiceNotInstalledException(service);

            var sc = ServiceHelper.GetServiceController(service);

            sc.Stop();

            using (MemoryStream ms = new MemoryStream(file))
            using (ZipFile zf = ZipFile.Read(ms))
            {
                zf.ExtractAll(path, ExtractExistingFileAction.OverwriteSilently);
            }

            File.WriteAllText(Path.Combine(path, module.Config.FileName), module.Config.Data);

            sc.Start();

            return service;
        }

        public Models.Service Remove(Module module)
        {
            String path = Path.Combine(this.moduleStore, $"{module.Name}");

            Models.Service service = new Models.Service()
            {
                Module = module,
                Name = module.Name,
                Path = path
            };

            if (!Directory.Exists(path))
                throw new ServiceNotInstalledException(service);

            ServiceHelper.Remove(service);

            return service;
        }

        #endregion

        #region Helper
        public List<Models.Service> ToInstall()
        {
            return null;
        }

        public List<Models.Service> ToUpdate()
        {
            return null;
        }

        public List<Models.Service> ToRemove()
        {
            return null;
        }
        #endregion

    }
}
