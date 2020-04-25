using SM.Models.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Security.Cryptography;
using SM.Models;
using Microsoft.AspNetCore.Authorization;

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
            customer.Customer_ID = Guid.NewGuid();
            customer.Auth_Token = this.GenerateAuthToken();

            customer.Name = Mapper.ExecuteScalar<String>("SELECT KDNAMI FROM KDSTM where KDKDNR = ? and KDWERK = ? and KDKZDK = 'D'",
                new OdbcParameter("kdnr", kdnr),
                new OdbcParameter("werk", _werk));

            Mapper.ExecuteQuery("INSERT INTO SM_Customers (Customer_ID, Kdnr, Auth_Token) VALUES (?,?,?)",
                new OdbcParameter("Customer_ID", customer.Customer_ID),
                new OdbcParameter("Kdnr", kdnr),
                new OdbcParameter("Auth_Token", customer.Auth_Token));

            return customer;
        }

        public void Update(Customer customer)
        {
            Mapper.ExecuteQuery("UPDATE SM_Customers SET Modified = now(), Auth_Token = ?, Kdnr = ? where Customer_ID = ?",
                new OdbcParameter("auth_token", customer.Auth_Token),
                new OdbcParameter("kdnr", customer.Kdnr),
                new OdbcParameter("customer_id", customer.Customer_ID));
        }

        public void Remove(Guid id)
        {
            Mapper.ExecuteQuery("UPDATE SM_Customers SET Deleted = now() where Customer_ID = ?",
                new OdbcParameter("customer_id", id));
        }

        public List<Customer> GetAll()
        {
            List<Customer> result = new List<Customer>();
            foreach (var cus in Mapper.GetMany<SM_Customers>("select SM_Customers.Customer_ID, SM_Customers.Kdnr, KDSTM.KDNAMI as Name, SM_Customers.Auth_Token from SM_Customers " +
                    "left join KDSTM on KDKDNR = SM_Customers.Kdnr and KDWERK = ? and KDKZDK = 'D' and KDSTAT <> 'L' where IsActive = 1",
                    new OdbcParameter("werk", _werk)))
            {
                result.Add(new Customer(cus));
            }
            return result;
        }

        public Customer Get(Guid id)
        {
            SM_Customers c = Mapper.GetSingle<SM_Customers>("select SM_Customers.Customer_ID, SM_Customers.Kdnr, KDSTM.KDNAMI as Name, SM_Customers.Auth_Token from SM_Customers " +
                "left join KDSTM on KDKDNR = SM_Customers.Kdnr and KDWERK = ? and KDKZDK = 'D' where SM_Customers.Customer_ID = ?",
                new OdbcParameter("werk", _werk),
                new OdbcParameter("Customer_ID", id));

            return new Customer(c);
        }

        public Guid GetCustomerId(Byte[] auth_token)
        {
            return Mapper.ExecuteScalar<Guid>("SELECT Customer_ID FROM SM_Customers where Auth_Token = ?",
                new OdbcParameter("auth_token", auth_token));
        }

        public Change AddChange(Guid customer_id, IDictionary<Module, ChangeItemOperation> modules)
        {
            Change change = new Change();
            change.Change_ID = Guid.NewGuid();
            change.Customer_ID = customer_id;
            change.Items = new List<ChangeItem>();

            foreach(var mod in modules)
            {
                change.Items.Add(this.AddChangeItem(customer_id, change.Change_ID, mod.Key, mod.Value));
            }

            Mapper.ExecuteQuery("INSERT INTO SM_Customers_Change (Change_ID, Customer_ID) VALUES (?,?)",
                new OdbcParameter("Change_id", change.Change_ID),
                new OdbcParameter("customer_id", change.Customer_ID));

            return change;
        }

        public Change GetChange(Guid change_id)
        {
            Change change = Mapper.GetSingle<Change>("SELECT * FROM SM_Customers_Change Where Change_ID = ?",
                new OdbcParameter("Change_ID", change_id));

            change.Items = Mapper.GetMany<ChangeItem>("SELECT * FROM SM_Customers_Change_Items where Change_ID = ?",
                new OdbcParameter("change_id", change_id));

            return change;
        }

        public List<Change> GetCustomerChanges(Guid customerId, Boolean selectDone = false)
        {
            return Mapper.GetMany<Change>($"SELECT * FROM SM_Customers_Change where Customer_ID = ? and (IsActive = 1 {(selectDone ? "or Changed is not null" : "")})",
                new OdbcParameter("customer_id", customerId));
        }

        public void RemoveChange(Guid change_id)
        {
            Mapper.ExecuteQuery("UPDATE SM_Customers_Change SET Deleted = now() where Change_ID = ?",
                new OdbcParameter("Change_id", change_id));
        }

        protected ChangeItem AddChangeItem(Guid customer_id, Guid change_id, Module module, ChangeItemOperation operation)
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
                this.AddModuleToCustomer(customer_id, module);
            }

            return item;
        }

        public void SetChange(Change change)
        {
            foreach (ChangeItem item in change.Items)
                SetChangeItem(item);

            Mapper.ExecuteQuery("UPDATE SM_Customers_Change SET Changed = now(), IsSuccess = ?, IsFailed = ?, IsWarning = ?, LogMessage = ? where Change_ID = ? and Customer_ID = ?",
                new OdbcParameter("isSuccess", change.IsSuccess ?? false),
                new OdbcParameter("isFailed", change.IsFailed ?? false),
                new OdbcParameter("isWarning", change.IsWarning ?? false),
                new OdbcParameter("logMessage", change.LogMessage),
                new OdbcParameter("customerId", change.Customer_ID));
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

        public void AddModuleToCustomer(Guid customer_id, Module module)
        {
            Mapper.ExecuteQuery("INSERT INTO SM_Customers_Modules (Customer_ID, Module_ID, Version, Status, Config) VALUES (?,?,?,?,?)",
                new OdbcParameter("Customer_ID", customer_id),
                new OdbcParameter("Module_ID", module.Module_ID),
                new OdbcParameter("Version", module.Version),
                new OdbcParameter("Status", ModuleStatus.Idle),
                new OdbcParameter("Config", module.Config.Data));
        }

        public void SetModuleStatus(Guid customer_id, Guid module_id, ModuleStatus status)
        {
            Mapper.ExecuteQuery("UPDATE SM_Customers_Modules SET Status = ? where Customer_ID = ? and Module_ID = ?",
                new OdbcParameter("Status", status),
                new OdbcParameter("Customer_ID", customer_id),
                new OdbcParameter("Module_ID", module_id));
        }

        public void RemovedModuleFromCustomer(Guid customer_id, Guid module_id)
        {
            Mapper.ExecuteQuery("Update SM_Customers_Modules SET Status = ?, Deleted = now() where Customer_ID = ? and Module_ID = ?",
                new OdbcParameter("Status", ModuleStatus.Uninstalled),
                new OdbcParameter("Customer_ID", customer_id),
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
