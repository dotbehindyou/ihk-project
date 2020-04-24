using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SM.Models
{
    public class Change
    {
        public Guid Change_ID { get; set; }
        public Guid Customer_ID { get; set; }
        public Boolean IsDone { get => Changed != null; }
        public DateTime? Changed { get; set; }
        public Boolean? IsSuccess { get => IsFailed == null && IsWarning == null ? null : (Boolean?)(!(IsFailed ?? false) ||!(IsWarning ?? false)); }
        public Boolean? IsFailed { get => Items?.Any(x=> x.IsFailed ?? false); }
        public Boolean? IsWarning { get => Items?.Any(x=> x.IsWarning ?? false); }
        public String LogMessage { get; set; }

        public List<ChangeItem> Items { get; set; }
    }
}
