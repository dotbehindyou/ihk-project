using System;
using System.Collections.Generic;
using SM.Models;
using System.Data.Odbc;
using System.Security.Cryptography;
using System.IO;
using SM.Models.Procedure;

namespace SM.Managers
{
    public class ModuleManager : BaseManager {
        public ModuleManager(BaseManager bm = null)
            : base(bm)
        {

        }

        private static String fileStorePath = Config.Current.FileStore;

        #region Module

        public Module Create (String moduleName)
        {
            Module module = new Module();
            module.Module_ID = Guid.NewGuid();
            module.Name = moduleName;

            Mapper.ExecuteQuery("DELETE SM_Modules where Name = ? and IsActive = 0", true,
                new OdbcParameter("name", moduleName));

            Mapper.ExecuteQuery("INSERT INTO SM_Modules (Module_ID, Name) VALUES (?,?)", true,
                new OdbcParameter("module_id", module.Module_ID),
                new OdbcParameter("name", module.Name));

            DirectoryInfo inf = new DirectoryInfo(module.Module_ID.ToString());

            if (!inf.Exists)
                inf.Create();

            return module;
        }

        public void Update (Module module)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules SET Modified = now(), Name = ? where Module_ID = ?;", true,
                new OdbcParameter("name", module.Name),
                new OdbcParameter("module_id", module.Module_ID));
        }

        public void Remove (Guid module_id)
        {
            Mapper.ExecuteQuery("UPDATE SM_Customers_Modules SET Status = 'DEL' where Module_ID = ?", true,
                new OdbcParameter("Module_ID", module_id)); // Status für Service, dass das Modul gelöscht werden soll 

            Mapper.ExecuteQuery("UPDATE SM_Modules SET Deleted = now() where Module_ID = ?", true,
                new OdbcParameter("module_id", module_id)); // Setzte das Modul als gelöscht
        }

        public List<Module> GetMany()
        {
            return Mapper.GetMany<Module>("select SM_Modules.Module_ID, Name, Version from SM_Modules " +
                "left join (select ROW_NUMBER() OVER (partition by Module_ID order by Release_Date desc) as rn, Version, Module_ID from SM_Modules_Version where IsActive = 1) as Versions on SM_Modules.Module_ID = Versions.Module_ID and rn = 1 " +
                "where IsActive = 1", true);
        }

        public List<Module> GetModulesFromCustomer(Int32 kdnr)
        {
            List<Module> mods = new List<Module>();

            foreach (var mod in Mapper.GetMany<Modules_Version_Config>("select mod.Module_ID, mod.Name as ModuleName, cus.Version, cus.Status, cof.Config_ID, cof.FileName as ConfigFileName, cus.Config as ConfigData, ver.Release_Date, ver.Validation_Token " +
                "from SM_Customers_Modules as cus " +
                        "inner join SM_Modules as mod on cus.Module_ID = mod.Module_ID and mod.IsActive = 1 " +
                        "left join SM_Modules_Version as ver on ver.Module_ID = cus.Module_ID and ver.Version = cus.Version " +
                        "left join SM_Modules_Config as cof on cof.Module_ID = mod.Module_ID and cof.Config_ID = ver.Config_ID and cof.IsActive = 1 " +
                        "where cus.Kdnr = ? and cus.IsActive = 1 and cus.Status <> 'DEL'", true,
                    new OdbcParameter("Kdnr", kdnr)))
            {
                mods.Add(new Module
                {
                    Module_ID = mod.Module_ID,
                    Name = mod.ModuleName,
                    Status = mod.Status,
                    Validation_Token = mod.Validation_Token,
                    Version = mod.Version,
                    Config = new ConfigFile
                    {
                        Config_ID = mod.Config_ID,
                        Data = mod.ConfigData,
                        FileName = mod.ConfigFileName,
                        Format = mod.ConfigFormat
                    }
                });
            }

            return mods;
        }

        public List<Module> GetModulesForService(Int32 kdnr)
        {
            List<Module> mods = new List<Module>();
            foreach(var mod in Mapper.GetMany<Modules_Version_Config>("select mod.Module_ID, mod.Name as ModuleName, cus.Version, cus.Status, cof.Config_ID, cof.FileName as ConfigFileName, cus.Config as ConfigData, ver.Release_Date, ver.Validation_Token from SM_Customers_Modules as cus " +
                    "inner join SM_Modules as mod on cus.Module_ID = mod.Module_ID " +
                    "left join SM_Modules_Version as ver on ver.Module_ID = cus.Module_ID and ver.Version = cus.Version " +
                    "left join SM_Modules_Config as cof on cof.Module_ID = mod.Module_ID and cof.Config_ID = ver.Config_ID " +
                    "where cus.Kdnr = ? and cus.IsActive = 1", true,
                    new OdbcParameter("kdnr", kdnr)))
            {
                mods.Add(new Module
                {
                    Module_ID = mod.Module_ID,
                    Name = mod.ModuleName,
                    Status = mod.Status,
                    Validation_Token = mod.Validation_Token,
                    Version = mod.Version,
                    Config = new ConfigFile
                    {
                        Config_ID = mod.Config_ID,
                        Data = mod.ConfigData,
                        FileName = mod.ConfigFileName,
                        Format = mod.ConfigFormat
                    }
                });
            }

            return mods;
        }

        public Module Get(Guid module_id)
        {
            return Mapper.GetSingle<Module>("select SM_Modules.Module_ID, Name, Version from SM_Modules " +
                "left join (select ROW_NUMBER() OVER (order by Release_Date desc) as rn, Version, SM_Modules_Version.Module_ID from SM_Modules_Version where Module_ID = ? and IsActive = 1) as Versions on SM_Modules.Module_ID = Versions.Module_ID and rn = 1" +
                "where SM_Modules.Module_ID = ?", true,
                new OdbcParameter("module_id", module_id),
                new OdbcParameter("module_id", module_id));
        }

        public void AddModuleToCustomer(Int32 kdnr, ModuleVersion version)
        {
            Mapper.ExecuteQuery("DELETE FROM SM_Customers_Modules WHERE Kdnr = ? and Module_ID = ?", true,
                new OdbcParameter("kdnr", kdnr),
                new OdbcParameter("Module_ID", version.Module_ID));

            Mapper.ExecuteQuery("INSERT INTO SM_Customers_Modules (Kdnr, Module_ID, Version, Config, Status) VALUES (?,?,?,?,?)", true,
                new OdbcParameter("kdnr", kdnr),
                new OdbcParameter("Module_ID", version.Module_ID),
                new OdbcParameter("Version", version.Version),
                new OdbcParameter("Config", version.Config.Data),
                new OdbcParameter("Status", "INIT"));
        }

        public void SetModuleToCustomer(Int32 kdnr, ModuleVersion version)
        {
            Mapper.ExecuteQuery("UPDATE SM_Customers_Modules SET Modified = now(), Version = ?, Config = ?, Status = ? WHERE Kdnr = ? and Module_ID = ?", true,
                new OdbcParameter("Version", version.Version),
                new OdbcParameter("Config", version.Config.Data),
                new OdbcParameter("Status", version.Status == "INIT" ? "INIT": "UPDATE"),
                new OdbcParameter("Kdnr", kdnr),
                new OdbcParameter("Module_ID", version.Module_ID));
        }

        public void RemoveModuleFromCustomer(Int32 kdnr, Guid module_id, Boolean delete = false)
        {
            Mapper.ExecuteQuery($"UPDATE SM_Customers_Modules SET {(delete ? "Deleted" : "Modified")} = now(), Status = ? WHERE Kdnr = ? and Module_ID = ?", true,
                new OdbcParameter("Status", "DEL"),
                new OdbcParameter("Kdnr", kdnr),
                new OdbcParameter("Module_ID", module_id));
        }

        public void SetModuleStatusFromCustomer(Int32 kdnr, Guid module_id, String status)
        {
            Mapper.ExecuteQuery($"UPDATE SM_Customers_Modules SET Modified = now(), Status = ? WHERE Kdnr = ? and Module_ID = ?", true,
                new OdbcParameter("Status", status),
                new OdbcParameter("Kdnr", kdnr),
                new OdbcParameter("Module_ID", module_id));
        }

        #endregion

        #region Version

        public IEnumerable<ModuleVersion> GetModuleVersions(Guid module_id)
        {
            return Mapper.GetMany<ModuleVersion>("SELECT SM_Modules_Version.Version, SM_Modules_Version.Module_ID, SM_Modules_Version.Validation_Token as ValidationToken, SM_Modules_Version.Release_Date as ReleaseDate, SM_Modules.Name as ModuleName, SM_Modules_Version.Config_ID as \"ConfigFile.Config_ID\" " +
                    "FROM SM_Modules_Version " +
                    "left join SM_Modules on SM_Modules.Module_ID = SM_Modules_Version.Module_ID " +
                    "where SM_Modules_Version.Module_ID = ? and SM_Modules_Version.IsActive = 1", true,
                new OdbcParameter("module_id", module_id));
        }

        public ModuleVersion GetVersionFromCustomer(Guid module_id, String version, Int32 kdnr)
        {
            var proc =  Mapper.GetSingle<Modules_Version_Config>("SELECT t_ver.Version, t_ver.Module_ID, t_mod.Name as ModuleName, Validation_Token as Validation_Token, t_ver.Release_Date as Release_Date, t_conf.Config_ID as Config_ID, t_conf.FileName as ConfigFileName, t_conf.Format as ConfigFormat, t_cus_mod.Config as  ConfigData " +
                    "FROM SM_Customers_Modules as t_cus_mod " +
                    "left join SM_Modules as t_mod on t_mod.Module_ID = t_cus_mod.Module_ID " +
                    "left join SM_Modules_Version as t_ver on t_cus_mod.Module_ID = t_ver.Module_ID and t_cus_mod.Version = t_ver.Version " +
                    "left join SM_Modules_Config as t_conf on t_conf.Config_ID = t_ver.Config_ID " +
                    "where t_cus_mod.Kdnr = ? and t_cus_mod.Module_ID = ? and t_cus_mod.Version = ?", true,
                new OdbcParameter("Kdnr", kdnr),
                new OdbcParameter("Module", module_id),
                new OdbcParameter("Version", version));

            return new ModuleVersion
            {
                Version = proc.Version,
                Module_ID = proc.Module_ID,
                ModuleName = proc.ModuleName,
                ReleaseDate = proc.Release_Date,
                ValidationToken = proc.Validation_Token,
                Config = new ConfigFile
                {
                    Config_ID = proc.Config_ID,
                    Data = proc.ConfigData,
                    FileName = proc.ConfigFileName,
                    Format = proc.ConfigFormat,
                }
            };
        }

        public ModuleVersion GetVersion(Guid module_id, String version)
        {
            var proc =  Mapper.GetSingle<Modules_Version_Config>("SELECT t_ver.Version, t_ver.Module_ID, t_mod.Name as ModuleName, Validation_Token as Validation_Token, t_ver.Release_Date as Release_Date, t_conf.Config_ID as Config_ID, t_conf.FileName as ConfigFileName, t_conf.Format as ConfigFormat, t_conf.Data as  ConfigData " +
                    "FROM SM_Modules_Version as t_ver " +
                    "left join SM_Modules_Config as t_conf on t_conf.Config_ID = t_ver.Config_ID " +
                    "left join SM_Modules as t_mod on t_mod.Module_ID = t_ver.Module_ID where t_ver.Module_ID = ? and t_ver.Version = ?", true,
                new OdbcParameter("Module", module_id),
                new OdbcParameter("Version", version));

            if(proc == null)
            {
                return null;
            }

            return new ModuleVersion
            {
                Version = proc.Version,
                Module_ID = proc.Module_ID,
                ModuleName = proc.ModuleName,
                ReleaseDate = proc.Release_Date,
                ValidationToken = proc.Validation_Token,
                Config = new ConfigFile
                {
                    Config_ID = proc.Config_ID,
                    Data = proc.ConfigData,
                    FileName = proc.ConfigFileName,
                    Format = proc.ConfigFormat,
                }
            };
        }

        public ModuleVersion AddVersion(Guid module_id, String version, ConfigFile configFile, DateTime releaseDate)
        {
            ModuleVersion ver = new ModuleVersion();
            ver.Module_ID = module_id;
            ver.Version = version;
            ver.ReleaseDate = releaseDate;
            ver.Version = version;
            ver.Module_ID = module_id;

            //if(versionFile != null)
            //    File.WriteAllBytes(Path.Combine(ver.Module_ID.ToString(), version + ".zip"), versionFile);

            if(configFile == null)
            {
                configFile = new ConfigFile()
                {
                    FileName = "config.json",
                    Format = "json",
                    Data = ""
                };
            }
            ver.Config = this.CreateConfig(module_id, configFile.FileName, configFile.Format, configFile.Data);

            Mapper.ExecuteQuery("DELETE SM_Modules_Version WHERE Module_ID = ? and Version = ? and IsActive = 0", true,
                new OdbcParameter("module_id", module_id),
                new OdbcParameter("Version", version));

            Mapper.ExecuteQuery("INSERT INTO SM_Modules_Version (Version, Module_ID, Validation_Token, Config_ID, Release_Date) " +
                "VALUES (?, ?, ?, ?, ?)", true,
                new OdbcParameter("Version", ver.Version),
                new OdbcParameter("Module_ID", ver.Module_ID),
                new OdbcParameter("Validation_Token", ver.ValidationToken ?? (Object)DBNull.Value),
                new OdbcParameter("Config_ID", ver.Config.Config_ID),
                new OdbcParameter("Release_Date", ver.ReleaseDate));


            return ver;
        }
        public ModuleVersion SetVersion(Guid module_id, String version, ModuleVersion ver)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules_Version SET Release_Date = ? where Module_ID = ? and Version = ?", true,
                new OdbcParameter("Release_Date", ver.ReleaseDate),
                new OdbcParameter("Module_ID", ver.Module_ID),
                new OdbcParameter("Version", ver.Version));

            return ver;
        }

        public void UpdateVersionFiles(Guid module_id, String version, Stream file)
        {
            Byte[] buffer;
            using (MemoryStream ms = new MemoryStream())
            {
                file.CopyTo(ms);
                buffer = ms.ToArray();
            }
            Byte[] validation_token;

            using (SHA512Managed man = new SHA512Managed()) {
                validation_token = man.ComputeHash(buffer);
            }

            DirectoryInfo di = new DirectoryInfo(Path.Combine(ModuleManager.fileStorePath, module_id.ToString()));
            if (!di.Exists)
                di.Create();

            File.WriteAllBytes(Path.Combine(di.FullName, version + ".zip"), buffer);

            buffer = null;

            Mapper.ExecuteQuery("UPDATE SM_Modules_Version SET Validation_Token = ?, Modified = now() where Version = ? and Module_ID = ?", true,
                new OdbcParameter("ValidationToken", validation_token),
                new OdbcParameter("version", version),
                new OdbcParameter("module_id", module_id));
        }

        public FileStream GetVersionFile(Guid module_id, Int32 kdnr, out Module module)
        {
            Module modVer =  Mapper.GetSingle<Module>("SELECT Version FROM SM_Customers_Modules as cu " +
                    "left join SM_Modules as mo on mo.Module_ID = cu.Module_ID and mo.IsActive = 1 " +
                    "WHERE cu.Kdnr = ? and cu.Module_ID = ? and cu.IsActive = 1", true,
                new OdbcParameter("kdnr", kdnr),
                new OdbcParameter("module_id", module_id));

            if (modVer == null || String.IsNullOrEmpty(modVer.Version))
                throw new Exception("Kunde hat keine Berechtigung für dieses Modul!");
            else
                module = modVer;

            var path = Path.Combine(ModuleManager.fileStorePath, module_id.ToString(), modVer.Version + ".zip");
            return File.OpenRead(path);
        }

        public void SetReleaseDate(Guid module_id, String version, DateTime releaseDate)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules_Version SET Release_Date = ?, Modified = now() where Version = ? and Module_ID = ?", true,
                new OdbcParameter("Release_Date", releaseDate),
                new OdbcParameter("version", version),
                new OdbcParameter("module_id", module_id));
        }

        public void SetConfig(Guid module_id, String version, Guid config_id)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules_Version SET Config_ID = ? where Version = ? and Module_ID = ?", true,
                new OdbcParameter("config_id", config_id),
                new OdbcParameter("version", version),
                new OdbcParameter("module_id", module_id));
        }

        public void RemoveVersion(Guid module_id, String version)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules_Version SET Deleted = now() where Version = ? and Module_ID = ?", true,
                new OdbcParameter("Version", version),
                new OdbcParameter("Module_ID", module_id));
        }

        #endregion

        #region Config

        public ConfigFile CreateConfig(Guid module_id, String configName, String format, String configData)
        {
            ConfigFile configFile = new ConfigFile();
            configFile.Config_ID = Guid.NewGuid();
            configFile.Data = configData;
            configFile.Format = String.IsNullOrEmpty(format) ? null : "";
            configFile.FileName = configName;

            Mapper.ExecuteQuery("INSERT INTO SM_Modules_Config (Config_ID, Module_ID, FileName, Format, Data) " +
                "VALUES (?,?,?,?,?)", true,
                new OdbcParameter("Config_ID", configFile.Config_ID),
                new OdbcParameter("Module_ID", module_id),
                new OdbcParameter("FileName", configFile.FileName),
                new OdbcParameter("Format", configFile.Format ?? (Object)DBNull.Value),
                new OdbcParameter("Data", configFile.Data));

            return configFile;
        }

        public void UpdateConfig(ConfigFile configFile)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules_Config SET Modified = now(), FileName = ?, Format = ?, Data = ? where Config_ID = ?", true,
                new OdbcParameter("FileName", configFile.FileName),
                new OdbcParameter("Format", String.IsNullOrEmpty(configFile.Format) ? (Object)DBNull.Value : configFile.Format),
                new OdbcParameter("Data", configFile.Data),
                new OdbcParameter("Config_ID", configFile.Config_ID));
        }

        public void RemoveConfig(Guid configId)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules_Config SET Deleted = now() where Config_ID = ?", true,
                new OdbcParameter("Config_ID", configId));
        }

        #endregion
    }
}
