using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Models
{
    public class Customer
    {
        public Guid Customer_ID { get; set; }
        public Int32 Kdnr { get; set; }
        public String Name { get; set; }
        public Byte[] Auth_Token { get; set; }
    }
}
