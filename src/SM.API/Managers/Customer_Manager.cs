using SM.Models.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Security.Cryptography;
using SM.Models;

namespace SM.API.Managers
{
    public class Customer_Manager : Base_Manager
    {
        public Customer Create(Int32 kdnr)
        {
            Customer customer = new Customer();
            customer.Kdnr = kdnr;
            customer.Customer_ID = Guid.NewGuid();
            customer.Auth_Token = this.GenerateAuthToken();

            customer.Name = Mapper.ExecuteScalar<String>("SELECT KDNAMI FROM KDSTM where KDKDNR = ? and KDWERK = ? and KDKZDK = 'D'",
                new OdbcParameter("kdnr", kdnr),
                new OdbcParameter("werk", Config.Werk));

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
            Mapper.ExecuteQuery("UPDATE SM_Customer SET Deleted = now() where Customer_ID = ?",
                new OdbcParameter("customer_id", id));
        }

        public List<Customer> GetAll()
        {
            return Mapper.GetMany<Customer>("select SM_Customers.Customer_ID, SM_Customers.Kdnr, KDSTM.KDNAMI as Name, SM_Customers.Auth_Token from SM_Customers " +
                "left join KDSTM on KDKDNR = SM_Customers.Kdnr and KDWERK = ? and KDKZDK = 'D' and KDSTAT <> 'L' where IsActive = 1",
                new OdbcParameter("werk", Config.Werk));
        }

        public Customer Get(Guid id)
        {
            return Mapper.GetSingle<Customer>("select SM_Customers.Customer_ID, SM_Customers.Kdnr, KDSTM.KDNAMI as Name, SM_Customers.Auth_Token from SM_Customers " +
                "left join KDSTM on KDKDNR = SM_Customers.Kdnr and KDWERK = ? and KDKZDK = 'D' where SM_Customers.Customer_ID = ?",
                new OdbcParameter("werk", Config.Werk),
                new OdbcParameter("Customer_ID", id));
        }

        public Change AddChange(Guid customer_id, IEnumerable<Module> modules)
        {
            Change change = new Change();
            change.Change_ID = Guid.NewGuid();
            change.Customer_ID = customer_id;
            change.Items = new List<ChangeItem>();

            foreach(Module mod in modules)
            {
                change.Items.Add(this.AddChangeItem(change.Change_ID, mod));
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

        protected ChangeItem AddChangeItem(Guid change_id, Module module)
        {
            ChangeItem item = new ChangeItem();
            item.Change_ID = change_id;
            item.Module_ID = module.Module_ID;
            item.Version = module.Version;

            Mapper.ExecuteQuery("INSERT INTO SM_Customers_Change_Items (Change_ID, Module_ID, Version) VALUES (?,?,?)",
                new OdbcParameter("change_id", change_id),
                new OdbcParameter("module_id", module.Module_ID),
                new OdbcParameter("version", module.Version));

            return item;
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
