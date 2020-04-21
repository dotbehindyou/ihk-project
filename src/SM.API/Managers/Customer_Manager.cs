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

        public void Delete(Guid id)
        {
            Mapper.ExecuteQuery("UPDATE SM_Customer SET Deleted = now() where Customer_ID = ?",
                new OdbcParameter("customer_id", id));
        }

        public List<SM_Customers> GetAll()
        {
            return Mapper.GetMany<SM_Customers>("select SM_Customers.Customer_ID, SM_Customers.Kdnr, KDSTM.KDNAMI as Name, SM_Customers.Auth_Token from SM_Customers " +
                "left join KDSTM on KDKDNR = SM_Customers.Kdnr and KDWERK = ? and KDKZDK = 'D' and KDSTAT <> 'L' where IsActive = 1",
                new OdbcParameter("werk", Config.Werk));
        }

        public SM_Customers Get(Guid id)
        {
            return Mapper.GetSingle<SM_Customers>("select SM_Customers.Customer_ID, SM_Customers.Kdnr, KDSTM.KDNAMI as Name, SM_Customers.Auth_Token from SM_Customers " +
                "left join KDSTM on KDKDNR = SM_Customers.Kdnr and KDWERK = ? and KDKZDK = 'D' where SM_Customers.Customer_ID = ?",
                new OdbcParameter("werk", Config.Werk),
                new OdbcParameter("Customer_ID", id));
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
