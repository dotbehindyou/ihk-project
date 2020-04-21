using System;
using System.Collections.Generic;
using System.Text;
using SM.Models;
using SM.Models.Table;
using System.Data.Odbc;

namespace SM.API.Managers
{
    public class Module_Manager : Base_Manager
    {
        public Module Create (String name)
        {
            Module module = new Module();
            module.Module_ID = Guid.NewGuid();
            module.Name = name;

            Mapper.ExecuteQuery("INSERT INTO SM_Modules (Module_ID, Name) VALUES (?,?)",
                new OdbcParameter("module_id", module.Module_ID),
                new OdbcParameter("name", module.Name));

            // TODO Module Ordner erstellen

            return module;
        }

        public void Update (SM_Modules module)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules SET Modified = now(), Name = ? where Module_ID = ?;",
                new OdbcParameter("name", module.Name),
                new OdbcParameter("module_id", module.Module_ID));
        }

        public void Delete (Guid module_id)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules SET Deleted = now() where Module_ID = ?",
                new OdbcParameter("module_id", module_id));
        }

        public List<Module> GetAll()
        {
            return Mapper.GetMany<Module>("select SM_Modules.Module_ID, Name, Version from SM_Modules " +
                "left join (select ROW_NUMBER() OVER (partition by Module_ID order by ReleaseDate desc) as rn, Version, Module_ID from SM_Modules_Version where IsActive = 1) as Versions on SM_Modules.Module_ID = Versions.Module_ID and rn = 1 " +
                "where IsActive = 1");
        }

        public Module Get(Guid module_id)
        {
            return Mapper.GetSingle<Module>("select SM_Modules.Module_ID, Name, Version from SM_Modules " +
                "left join (select ROW_NUMBER() OVER (order by ReleaseDate desc) as rn, Version, Module_ID from SM_Modules_Version where Module_ID = ? and IsActive = 1) as Versions on SM_Modules.Module_ID = Versions.Module_ID and rn = 1" +
                "where Module_ID = ?",
                new OdbcParameter("module_id", module_id),
                new OdbcParameter("module_id", module_id));
        }

        public Module AddVersion(Guid module_id, String version, ConfigFile config, Byte[] file)
        {
            Module module = new Module();
            module.Module_ID = module_id;
            module.Version = version;
            module.Config = config;

            Mapper.ExecuteQuery("");

            // TODO Versions Datei speichern

            return module;
        }

        public void SetVersion()
        {

        }

        public void RemoveVersion()
        {

        }
    }
}
