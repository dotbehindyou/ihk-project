using System;
using System.Collections.Generic;
using System.Text;
using SM.Models;
using SM.Models.Table;
using System.Data.Odbc;
using System.Security.Cryptography;
using System.IO;
using SM.Models.Procedure;

namespace SM.API.Managers
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

        public void Update (SM_Modules module)
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

        public List<Module> GetAll()
        {
            return Mapper.GetMany<Module>("select SM_Modules.Module_ID, Name, Version from SM_Modules " +
                "left join (select ROW_NUMBER() OVER (partition by Module_ID order by Release_Date desc) as rn, Version, Module_ID from SM_Modules_Version where IsActive = 1) as Versions on SM_Modules.Module_ID = Versions.Module_ID and rn = 1 " +
                "where IsActive = 1");
        }

        public Module Get(Guid module_id)
        {
            return Mapper.GetSingle<Module>("select SM_Modules.Module_ID, Name, Version from SM_Modules " +
                "left join (select ROW_NUMBER() OVER (order by Release_Date desc) as rn, Version, Module_ID from SM_Modules_Version where Module_ID = ? and IsActive = 1) as Versions on SM_Modules.Module_ID = Versions.Module_ID and rn = 1" +
                "where Module_ID = ?",
                new OdbcParameter("module_id", module_id),
                new OdbcParameter("module_id", module_id));
        }

        #endregion

        #region Version

        public IEnumerable<ModuleVersion> GetModuleVersions(Guid module_id)
        {
            return Mapper.GetMany<ModuleVersion>("SELECT Version, Module_ID, Validation_Token as ValidationToken, ReleaseDate FROM SM_Modules_Version where Module_ID = ?",
                new OdbcParameter("module_id", module_id));
        }

        public ModuleVersion GetVersion(Guid module_id, String version)
        {
            var proc = Mapper.GetSingle<Modules_Version_Config>("SELECT t_ver.Version, t_ver.Module_ID, t_mod.Name as ModuleName, Validation_Token as ValidationToken, t_ver.Release_Date as ReleaseDate, " +
                "t_conf.FileName as ConfigFileName, t_conf.Format as ConfigFormat, t_conf.Data as ConfigData " +
                "FROM SM_Modules_Version  as t_ver " +
                "left join SM_Modules_Config as t_conf on t_conf.Config_ID = t_ver.Config_ID " +
                "left join SM_Modules as t_mod on t_mod.Module_ID = t_ver.Module_ID",
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

        public ModuleVersion AddVersion(Guid module_id, String version, Byte[] zipFile, DateTime releaseDate)
        {
            ModuleVersion ver = new ModuleVersion();
            ver.Module_ID = module_id;
            ver.Version = version;

            File.WriteAllBytes(Path.Combine(ver.Module_ID.ToString(), version + ".zip"), zipFile);

            Mapper.ExecuteQuery("INSERT INTO SM_Modules_Version (Version, Module_ID, Validation_Token, Config_ID, Release_Date) " +
                "VALUES (?, ?, ?, ?, ?)",
                new OdbcParameter("Version", version),
                new OdbcParameter("Module_ID", module_id),
                new OdbcParameter("Validation_Token", ver.ValidationToken),
                new OdbcParameter("Release_Date", releaseDate));

            return ver;
        }

        public void UpdateVersion(Guid module_id, String version, Byte[] zipFile)
        {
            Byte[] validation_token;
            using (SHA512Managed man = new SHA512Managed())
            {
                validation_token = man.ComputeHash(zipFile);
            }

            File.WriteAllBytes(Path.Combine(module_id.ToString(), version + ".zip"), zipFile);

            Mapper.ExecuteQuery("UPDATE SM_Modules_Version SET ValidationToken = ?, Config_ID = ?, Modified = now(), Release_Date = ? where Version = ? and Module_ID = ?",
                new OdbcParameter("ValidationToken", validation_token));
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
            Mapper.ExecuteQuery("UPDATE SM_Modules_Version SET Config_ID = ?, Modified = now() where Version = ? and Module_ID",
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

            Mapper.ExecuteQuery("INSERT INTO SM_Modules_Config (Config_ID, Module_ID, FileName, Format, Data) " +
                "VALUES (?,?,?,?,?)",
                new OdbcParameter("Config_ID", configFile.Config_ID),
                new OdbcParameter("Module_ID", module_id),
                new OdbcParameter("FileName", configFile),
                new OdbcParameter("Format", configFile.Format),
                new OdbcParameter("Data", configFile.Data));

            return configFile;
        }

        public void UpdateConfig(ConfigFile configFile)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules_Config SET Modified = now(), FileName = ?, Format = ?, Data = ? where Config_ID = ?",
                new OdbcParameter("FileName", configFile.FileName),
                new OdbcParameter("Format", configFile.Format),
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
