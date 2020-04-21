using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Models.Table
{
    public class SM_Customers_Change_Items
    {
        public Guid Change_ID { get; set; }
        public Guid Module_ID { get; set; }
        public String Version { get; set; }
        public Boolean IsSuccess { get; set; }
        public Boolean IsFailed { get; set; }
        public Boolean IsWarning { get; set; }
        public DateTime Changed { get; set; }
    }
}
