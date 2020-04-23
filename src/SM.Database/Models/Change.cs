using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Models
{
    public class Change
    {
        public Guid Change_ID { get; set; }
        public Guid Customer_ID { get; set; }
        public Boolean IsDone { get => Changed != null; }
        public DateTime? Changed { get; set; }
        public Boolean? IsSuccess { get; set; }
        public Boolean? IsFailed { get; set; }
        public Boolean? IsWarning { get; set; }
        public String LogMessage { get; set; }

        public List<ChangeItem> Items { get; set; }
    }
}
