using Newtonsoft.Json;
using SM.Models.Table;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SM.Models
{
    public class Customer
    {
        public Customer()
        {

        }

        public Customer(SM_Customers customers)
        {
            this.Kdnr = customers.Kdnr;
            this.Name = customers.Name;
            Auth_Token = customers.Auth_Token;
        }

        public Int32 Kdnr { get; set; }
        public String Name { get; set; }
        public Byte[] Auth_Token { get; set; }
    }
}
