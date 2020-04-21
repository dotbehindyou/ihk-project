using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Models.Table
{
    public class SM_Customers_Change
    {
        public Guid Change_ID { get; set; }
        public Guid Customer_ID { get; set; }
        public DateTime Changed { get; set; }
        public Boolean IsSuccess { get; set; }
        public Boolean IsFailed { get; set; }
        public Boolean IsWarning { get; set; }
        public String LogMessage { get; set; }
        public DateTime Created { get; set; }
        public DateTime Deleted { get; set; }
        public Boolean IsActive { get; set; }
    }
}
