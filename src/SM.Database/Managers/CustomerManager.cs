using SM.Models.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Odbc;
using System.Security.Cryptography;
using SM.Models;
using System.Data.Common;

namespace SM.Managers
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
            foreach (var cus in Mapper.GetMany<SM_Customers>("select top 1000 KDKDnr as Kdnr, KDSTM.KDNAMI as Name, SM_Customers.Auth_Token, cast(case when SM_Customers.Kdnr is null then 0 else 1 end as bit) as IsRegisterd from KDSTM " +
                        "left outer join SM_Customers on KDKDNR = SM_Customers.Kdnr and IsActive = 1 where KDWERK = ? and KDKZDK = 'D' and KDSTAT <> 'L' " +
                        $"{ (String.IsNullOrEmpty(search.Name) ? "" : "and Name like '%'+?+'%'") } { (search.Kdnr == null ? "" : "and KDKDNR " + (search.KdnrCondition == SearchCondition.Same ? "= ?"  : "like '%'+?+'%'"))} " +
                        "order by IsRegisterd desc, Kdnr", parms.ToArray()))
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

        public Byte[] GenerateAuthToken()
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                return shaM.ComputeHash(Encoding.UTF8.GetBytes($"{DateTime.Now.ToBinary()}{Guid.NewGuid()}"));
            }
        }
    }
}
