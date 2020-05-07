using SM.Models.Table;
using SM.Managers;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Text;
using System.Threading;

namespace SM.Managers {
    public class CustomerServiceManager : BaseManager {
        public CustomerServiceManager(BaseManager manager = null)
            :base(manager)
        {

        }

        public List<SM_Modules_Installed> GetMany()
        {
            return Mapper.GetMany<SM_Modules_Installed>("SELECT * FROM SM_Modules_Installed where IsActive = 1", true);
        }

        public List<SM_Modules_Installed> Get(Guid module_id)
        {
            return Mapper.GetMany<SM_Modules_Installed>("SELECT * FROM SM_Modules_Installed where Module_ID = 1", true,
                new OdbcParameter("Module_ID", module_id));
        }

        public void Add(SM_Modules_Installed mod)
        {
            Mapper.ExecuteQuery("INSERT INTO SM_Modules_Installed (Module_ID, ServiceName, Version, ValidationToken, ModuleName, Path)" +
                "VALUES(?,?,?,?,?,?)", true,
                new OdbcParameter("Module_ID", mod.Module_ID),
                new OdbcParameter("ServiceName", mod.ServiceName),
                new OdbcParameter("Version", mod.Version),
                new OdbcParameter("ValidationToken", mod.ValidationToken),
                new OdbcParameter("ModuleName", mod.ModuleName),
                new OdbcParameter("Path", mod.Path));
        }

        public void Update(SM_Modules_Installed mod)
        {
            Mapper.ExecuteQuery("UPDATE SM_MOdules_Installed SET Modified = now(), ServiceName = ?, Version = ?, ValidationToken = ?, ModuleName = ? where Module_ID = ?", true,
                new OdbcParameter("ServiceName", mod.ServiceName),
                new OdbcParameter("Version", mod.Version),
                new OdbcParameter("ValidationToken", mod.ValidationToken),
                new OdbcParameter("ModuleName", mod.ModuleName),
                new OdbcParameter("Module_ID", mod.Module_ID));
        }

        public void Remove(Guid module_id)
        {
            Mapper.ExecuteQuery("UPDATE SM_Modules_Installed SET Deleted = now() where Module_ID = ?", true,
                new OdbcParameter("Module_ID", module_id));
        }
    }
}
