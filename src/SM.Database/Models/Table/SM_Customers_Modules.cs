using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Models.Table
{
    public class SM_Customers_Modules
    {
        public Guid Customer_ID { get; set; }
        public Guid Module_ID { get; set; }
        public String Version { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Deleted { get; set; }
        public Boolean IsActive { get; set; }
        public String Status { get; set; }
        public String Config { get; set; }
    }
}
