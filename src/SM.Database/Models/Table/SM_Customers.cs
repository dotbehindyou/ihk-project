using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace SM.Models.Table
{
    public class SM_Customers
    {
        public Int32 Kdnr { get; set; }
        public Byte[] Auth_Token { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Deleted { get; set; }
        public Boolean IsActive { get; set; }

        public String Name { get; set; }
    }
}
