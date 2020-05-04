using System;
using System.Collections.Generic;
using System.Text;
using SM.Models;
using SM.Models.Table;
using System.Data.Odbc;
using System.Security.Cryptography;
using System.IO;
using SM.Models.Procedure;

namespace SM.Managers
{
    public class ModuleManager : BaseManager
    {
        public ModuleManager(string connectionString) : base(connectionString)
        {
        }

        #region Module

        public Module Create (String moduleName)
        {
            Module module = new Module();
            module.Module_ID = Guid.NewGuid();
            module.Name = moduleName;

            Mapper.ExecuteQuery("INSERT INTO SM_Modules (Module_ID, Name) VALUES (?,?)",
                new OdbcParameter("module_id", module.Module_ID),
                new OdbcParameter("name", module.Name));

            DirectoryInfo inf = new DirectoryInfo(module.Module_ID.ToString());

            if (!inf.Exists)
                inf.Create();

            return module;
        }

        public void Update (Module module)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules SET Modified = now(), Name = ? where Module_ID = ?;",
                new OdbcParameter("name", module.Name),
                new OdbcParameter("module_id", module.Module_ID));
        }

        public void Remove (Guid module_id)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules SET Deleted = now() where Module_ID = ?",
                new OdbcParameter("module_id", module_id));
        }

        public List<Module> GetMany()
        {
            return Mapper.GetMany<Module>("select SM_Modules.Module_ID, Name, Version from SM_Modules " +
                "left join (select ROW_NUMBER() OVER (partition by Module_ID order by Release_Date desc) as rn, Version, Module_ID from SM_Modules_Version where IsActive = 1) as Versions on SM_Modules.Module_ID = Versions.Module_ID and rn = 1 " +
                "where IsActive = 1");
        }

        public List<Module> GetModulesFromCustomer(Int32 kdnr)
        {
            return Mapper.GetMany<Module>("select mod.Module_ID, mod.Name, cus.Version, cus.Status from SM_Customers_Modules as cus " +
                "left join SM_Modules as mod on cus.Module_ID = mod.Module_ID and mod.IsActive = 1" +
                "where cus.Kdnr = ? and cus.IsActive = 1",
                new OdbcParameter("kdnr", kdnr));
        }

        public Module Get(Guid module_id)
        {
            return Mapper.GetSingle<Module>("select SM_Modules.Module_ID, Name, Version from SM_Modules " +
                "left join (select ROW_NUMBER() OVER (order by Release_Date desc) as rn, Version, SM_Modules_Version.Module_ID from SM_Modules_Version where Module_ID = ? and IsActive = 1) as Versions on SM_Modules.Module_ID = Versions.Module_ID and rn = 1" +
                "where SM_Modules.Module_ID = ?",
                new OdbcParameter("module_id", module_id),
                new OdbcParameter("module_id", module_id));
        }

        public void AddModuleToCustomer(Int32 kdnr, ModuleVersion version)
        {
            Mapper.ExecuteQuery("DELETE FROM SM_Customers_Modules WHERE Kdnr = ? and Module_ID = ?",
                new OdbcParameter("kdnr", kdnr),
                new OdbcParameter("Module_ID", version.Module_ID));

            Mapper.ExecuteQuery("INSERT INTO SM_Customers_Modules (Kdnr, Module_ID, Version, Config, Status) VALUES (?,?,?,?,?)",
                new OdbcParameter("kdnr", kdnr),
                new OdbcParameter("Module_ID", version.Module_ID),
                new OdbcParameter("Version", version.Version),
                new OdbcParameter("Config", version.Config.Data),
                new OdbcParameter("Status", "INIT"));
        }

        public void SetModuleToCustomer(Int32 kdnr, ModuleVersion version)
        {
            Mapper.ExecuteQuery("UPDATE SM_Customers_Modules SET Modified = now(), Version = ?, Config = ?, Status = ? WHERE Kdnr = ? and Module_ID = ?",
                new OdbcParameter("Version", version.Version),
                new OdbcParameter("Config", version.Config.Data),
                new OdbcParameter("Status", "UPDATE"),
                new OdbcParameter("Kdnr", kdnr),
                new OdbcParameter("Module_ID", version.Module_ID));
        }

        public void RemoveModuleFromCustomer(Int32 kdnr, ModuleVersion version)
        {
            Mapper.ExecuteQuery("UPDATE SM_Customers_Modules SET Modified = now(), Status = ? WHERE Kdnr = ? and Module_ID = ?",
                new OdbcParameter("Status", "REMOVE"),
                new OdbcParameter("Kdnr", kdnr),
                new OdbcParameter("Module_ID", version.Module_ID));
        }

        public void SetModuleStatusFromCustomer(Int32 kdnr, ModuleVersion version)
        {
            Mapper.ExecuteQuery($"UPDATE SM_Customers_Modules SET {(version.Status == "REMOVED" ? "Deleted" : "Modified")} = now(), Status = ? WHERE Kdnr = ? and Module_ID = ?",
                new OdbcParameter("Status", version.Status),
                new OdbcParameter("Kdnr", kdnr),
                new OdbcParameter("Module_ID", version.Module_ID));
        }

        #endregion

        #region Version

        public IEnumerable<ModuleVersion> GetModuleVersions(Guid module_id)
        {
            return Mapper.GetMany<ModuleVersion>("SELECT SM_Modules_Version.Version, SM_Modules_Version.Module_ID, SM_Modules_Version.Validation_Token as ValidationToken, SM_Modules_Version.Release_Date as ReleaseDate, SM_Modules.Name as ModuleName, SM_Modules_Version.Config_ID as \"ConfigFile.Config_ID\" " +
                    "FROM SM_Modules_Version " +
                    "left join SM_Modules on SM_Modules.Module_ID = SM_Modules_Version.Module_ID " +
                    "where SM_Modules_Version.Module_ID = ? and SM_Modules_Version.IsActive = 1",
                new OdbcParameter("module_id", module_id));
        }

        public ModuleVersion GetVersionFromCustomer(Guid module_id, String version, Int32 kdnr)
        {
            var proc = Mapper.GetSingle<Modules_Version_Config>("SELECT t_ver.Version, t_ver.Module_ID, t_mod.Name as ModuleName, Validation_Token as Validation_Token, t_ver.Release_Date as Release_Date, t_conf.Config_ID as Config_ID, t_conf.FileName as ConfigFileName, t_conf.Format as ConfigFormat, t_cus_mod.Config as  ConfigData " +
                    "FROM SM_Customers_Modules as t_cus_mod " +
                    "left join SM_Modules as t_mod on t_mod.Module_ID = t_cus_mod.Module_ID " +
                    "left join SM_Modules_Version as t_ver on t_cus_mod.Module_ID = t_ver.Module_ID and t_cus_mod.Version = t_ver.Version " +
                    "left join SM_Modules_Config as t_conf on t_conf.Config_ID = t_ver.Config_ID " +
                    "where t_cus_mod.Kdnr = ? and t_cus_mod.Module_ID = ? and t_cus_mod.Version = ?",
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
            var proc = Mapper.GetSingle<Modules_Version_Config>("SELECT t_ver.Version, t_ver.Module_ID, t_mod.Name as ModuleName, Validation_Token as Validation_Token, t_ver.Release_Date as Release_Date, t_conf.Config_ID as Config_ID, t_conf.FileName as ConfigFileName, t_conf.Format as ConfigFormat, t_conf.Data as  ConfigData " +
                    "FROM SM_Modules_Version as t_ver " +
                    "left join SM_Modules_Config as t_conf on t_conf.Config_ID = t_ver.Config_ID " +
                    "left join SM_Modules as t_mod on t_mod.Module_ID = t_ver.Module_ID where t_ver.Module_ID = ? and t_ver.Version = ?",
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

            ver.Config = this.CreateConfig(module_id, configFile.FileName, configFile.Format, configFile.Data);

            Mapper.ExecuteQuery("INSERT INTO SM_Modules_Version (Version, Module_ID, Validation_Token, Config_ID, Release_Date) " +
                "VALUES (?, ?, ?, ?, ?)",
                new OdbcParameter("Version", ver.Version),
                new OdbcParameter("Module_ID", ver.Module_ID),
                new OdbcParameter("Validation_Token", ver.ValidationToken ?? (Object)DBNull.Value),
                new OdbcParameter("Config_ID", ver.Config.Config_ID),
                new OdbcParameter("Release_Date", ver.ReleaseDate));


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

            DirectoryInfo di = new DirectoryInfo("storage/" + module_id.ToString());
            if (!di.Exists)
                di.Create();

            File.WriteAllBytes(Path.Combine(di.FullName, version + ".zip"), buffer);

            buffer = null;

            Mapper.ExecuteQuery("UPDATE SM_Modules_Version SET Validation_Token = ?, Modified = now() where Version = ? and Module_ID = ?",
                new OdbcParameter("ValidationToken", validation_token),
                new OdbcParameter("version", version),
                new OdbcParameter("module_id", module_id));
        }

        public FileStream GetVersionFile(Guid module_id, String version)
        {
            var path = Path.Combine("storage", module_id.ToString(), version + ".zip");
            return File.OpenRead(path);
        }

        public void SetReleaseDate(Guid module_id, String version, DateTime releaseDate)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules_Version SET Release_Date = ?, Modified = now() where Version = ? and Module_ID = ?",
                new OdbcParameter("Release_Date", releaseDate),
                new OdbcParameter("version", version),
                new OdbcParameter("module_id", module_id));
        }

        public void SetConfig(Guid module_id, String version, Guid config_id)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules_Version SET Config_ID = ? where Version = ? and Module_ID = ?",
                new OdbcParameter("config_id", config_id),
                new OdbcParameter("version", version),
                new OdbcParameter("module_id", module_id));
        }

        public void RemoveVersion(Guid module_id, String version)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules_Version SET Deleted = now() where Version = ? and Module_ID = ?",
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
            configFile.Format = format;
            configFile.FileName = configName;

            Mapper.ExecuteQuery("INSERT INTO SM_Modules_Config (Config_ID, Module_ID, FileName, Format, Data) " +
                "VALUES (?,?,?,?,?)",
                new OdbcParameter("Config_ID", configFile.Config_ID),
                new OdbcParameter("Module_ID", module_id),
                new OdbcParameter("FileName", configFile.FileName),
                new OdbcParameter("Format", configFile.Format ?? ""),
                new OdbcParameter("Data", configFile.Data));

            return configFile;
        }

        public void UpdateConfig(ConfigFile configFile)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules_Config SET Modified = now(), FileName = ?, Format = ?, Data = ? where Config_ID = ?",
                new OdbcParameter("FileName", configFile.FileName),
                new OdbcParameter("Format", configFile.Format ?? (Object)DBNull.Value),
                new OdbcParameter("Data", configFile.Data),
                new OdbcParameter("Config_ID", configFile.Config_ID));
        }

        public void RemoveConfig(Guid configId)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules_Config SET Deleted = now() where Config_ID = ?",
                new OdbcParameter("Config_ID", configId));
        }

        #endregion
    }
}
