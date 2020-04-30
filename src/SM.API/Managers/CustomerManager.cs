using SM.Models.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Security.Cryptography;
using SM.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data.Common;

namespace SM.API.Managers
{
    public class CustomerManager : BaseManager
    {
        private readonly String _werk;

        public CustomerManager(String connectionString, String werk) : base(connectionString)
        {
            this._werk = werk;
        }

        public Customer Create(Int32 kdnr)
        {
            Customer customer = new Customer();
            customer.Kdnr = kdnr;
            customer.Auth_Token = this.GenerateAuthToken();

            customer.Name = Mapper.ExecuteScalar<String>("SELECT KDNAMI FROM KDSTM where KDKDNR = ? and KDWERK = ? and KDKZDK = 'D'",
                new OdbcParameter("kdnr", kdnr),
                new OdbcParameter("werk", _werk));

            Mapper.ExecuteQuery("INSERT INTO SM_Customers (Kdnr, Auth_Token) VALUES (?,?)",
                new OdbcParameter("Kdnr", kdnr),
                new OdbcParameter("Auth_Token", customer.Auth_Token));

            return customer;
        }

        public void Update(Customer customer)
        {
            Mapper.ExecuteQuery("UPDATE SM_Customers SET Modified = now(), Auth_Token = ? where Kdnr = ?",
                new OdbcParameter("auth_token", customer.Auth_Token),
                new OdbcParameter("kdnr", customer.Kdnr));
        }

        public void Remove(Int32 kdnr)
        {
            Mapper.ExecuteQuery("UPDATE SM_Customers SET Deleted = now() where kdnr = ?",
                new OdbcParameter("kdnr", kdnr));
        }

        public List<Customer> GetMany(Search search = null)
        {
            if (search == null)
                search = new Search();

            var parms = new List<DbParameter>();
            parms.Add(new OdbcParameter("werk", _werk));
            if(!String.IsNullOrEmpty(search.Name))
                parms.Add(new OdbcParameter("Name", search.Name));
            if(search.Kdnr != null)
                parms.Add(new OdbcParameter("Kdnr", search.Kdnr.ToString()));

            List<Customer> result = new List<Customer>();
            foreach (var cus in Mapper.GetMany<SM_Customers>("select top 1000 KDKDnr as Kdnr, KDSTM.KDNAMI as Name, SM_Customers.Auth_Token, case when SM_Customers.Kdnr is null then 0 else 1 end as IsRegisterd from KDSTM " +
                        "left outer join SM_Customers on KDKDNR = SM_Customers.Kdnr and IsActive = 1 where KDWERK = ? and KDKZDK = 'D' and KDSTAT <> 'L' " +
                        $"{ (String.IsNullOrEmpty(search.Name) ? "" : "and Name like '%'+?+'%'") } { (search.Kdnr == null ? "" : "and KDKDNR " + (search.KdnrCondition == SearchCondition.Same ? "= ?"  : "like '%'+?+'%'"))} " +
                        "order by IsRegisterd desc", parms.ToArray()))
            {
                result.Add(new Customer(cus));
            }
            return result;
        }

        public Customer Get(Int32 kdnr)
        {
            SM_Customers c = Mapper.GetSingle<SM_Customers>("select SM_Customers.Kdnr, KDSTM.KDNAMI as Name, SM_Customers.Auth_Token from SM_Customers " +
                "left join KDSTM on KDKDNR = SM_Customers.Kdnr and KDWERK = ? and KDKZDK = 'D' where SM_Customers.Kdnr = ?",
                new OdbcParameter("werk", _werk),
                new OdbcParameter("Kdnr", kdnr));

            return new Customer(c);
        }

        public Int32 GetCustomerKdnr(Byte[] auth_token)
        {
            return Mapper.ExecuteScalar<Int32>("SELECT Kdnr FROM SM_Customers where Auth_Token = ?",
                new OdbcParameter("auth_token", auth_token));
        }

        public Change AddChange(Int32 kdnr, IDictionary<Module, ChangeItemOperation> modules)
        {
            Change change = new Change();
            change.Change_ID = Guid.NewGuid();
            change.Items = new List<ChangeItem>();

            foreach(var mod in modules)
            {
                change.Items.Add(this.AddChangeItem(kdnr, change.Change_ID, mod.Key, mod.Value));
            }

            Mapper.ExecuteQuery("INSERT INTO SM_Customers_Change (Change_ID, Kdnr) VALUES (?,?)",
                new OdbcParameter("Change_id", change.Change_ID),
                new OdbcParameter("Kdnr", change.Kdnr));

            return change;
        }

        public Change GetChange(Int32 kdnr)
        {
            Change change = Mapper.GetSingle<Change>("SELECT * FROM SM_Customers_Change Where kdnr = ?",
                new OdbcParameter("kdnr", kdnr));

            change.Items = Mapper.GetMany<ChangeItem>("SELECT * FROM SM_Customers_Change_Items where kdnr = ?",
                new OdbcParameter("kdnr", kdnr));

            return change;
        }

        public List<Change> GetCustomerChanges(Int32 kdnr, Boolean selectDone = false)
        {
            return Mapper.GetMany<Change>($"SELECT * FROM SM_Customers_Change where Kdnr = ? and (IsActive = 1 {(selectDone ? "or Changed is not null" : "")})",
                new OdbcParameter("kdnr", kdnr));
        }

        public void RemoveChange(Guid change_id)
        {
            Mapper.ExecuteQuery("UPDATE SM_Customers_Change SET Deleted = now() where Change_ID = ?",
                new OdbcParameter("Change_id", change_id));
        }

        protected ChangeItem AddChangeItem(Int32 kdnr, Guid change_id, Module module, ChangeItemOperation operation)
        {
            ChangeItem item = new ChangeItem();
            item.Change_ID = change_id;
            item.Module_ID = module.Module_ID;
            item.Version = module.Version;

            Mapper.ExecuteQuery("INSERT INTO SM_Customers_Change_Items (Change_ID, Module_ID, Version) VALUES (?,?,?)",
                new OdbcParameter("change_id", change_id),
                new OdbcParameter("module_id", module.Module_ID),
                new OdbcParameter("version", module.Version));

            if(operation == ChangeItemOperation.Install)
            {
                this.AddModuleToCustomer(kdnr, module);
            }

            return item;
        }

        public void SetChange(Change change)
        {
            foreach (ChangeItem item in change.Items)
                SetChangeItem(item);

            Mapper.ExecuteQuery("UPDATE SM_Customers_Change SET Changed = now(), IsSuccess = ?, IsFailed = ?, IsWarning = ?, LogMessage = ? where Change_ID = ? and kdnr = ?",
                new OdbcParameter("isSuccess", change.IsSuccess ?? false),
                new OdbcParameter("isFailed", change.IsFailed ?? false),
                new OdbcParameter("isWarning", change.IsWarning ?? false),
                new OdbcParameter("logMessage", change.LogMessage),
                new OdbcParameter("kdnr", change.Kdnr));
        }

        protected void SetChangeItem(ChangeItem item)
        {
            Mapper.ExecuteQuery("UPDATE SM_Customers_Change_Items SET Changed = now(), IsSuccess = ?, IsFailed = ?, IsWarning = ? where Change_ID = ? and Module_ID = ? and Version = ?",
                new OdbcParameter("isSuccess", item.IsSuccess ?? false),
                new OdbcParameter("isFailed", item.IsFailed ?? false),
                new OdbcParameter("isWarning", item.IsWarning ?? false),
                new OdbcParameter("Change_ID", item.Change_ID),
                new OdbcParameter("Module_ID", item.Module_ID),
                new OdbcParameter("Version", item.Version));
        }

        public void AddModuleToCustomer(Int32 kdnr, Module module)
        {
            Mapper.ExecuteQuery("INSERT INTO SM_Customers_Modules (kdnr, Module_ID, Version, Status, Config) VALUES (?,?,?,?,?)",
                new OdbcParameter("kdnr", kdnr),
                new OdbcParameter("Module_ID", module.Module_ID),
                new OdbcParameter("Version", module.Version),
                new OdbcParameter("Status", ModuleStatus.Idle),
                new OdbcParameter("Config", module.Config.Data));
        }

        public void SetModuleStatus(Int32 kdnr, Guid module_id, ModuleStatus status)
        {
            Mapper.ExecuteQuery("UPDATE SM_Customers_Modules SET Status = ? where kdnr = ? and Module_ID = ?",
                new OdbcParameter("Status", status),
                new OdbcParameter("kdnr", kdnr),
                new OdbcParameter("Module_ID", module_id));
        }

        public void RemovedModuleFromCustomer(Int32 kdnr, Guid module_id)
        {
            Mapper.ExecuteQuery("Update SM_Customers_Modules SET Status = ?, Deleted = now() where kdnr = ? and Module_ID = ?",
                new OdbcParameter("Status", ModuleStatus.Uninstalled),
                new OdbcParameter("kdnr", kdnr),
                new OdbcParameter("Module_ID", module_id));
        }

        public Byte[] GenerateAuthToken()
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                return shaM.ComputeHash(Encoding.UTF8.GetBytes($"{DateTime.Now.ToBinary()}{Guid.NewGuid()}"));
            }
        }
    }
}
