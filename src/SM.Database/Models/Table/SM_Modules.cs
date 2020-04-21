using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Models.Table
{
    public class SM_Modules
    {
        public Guid Module_ID { get; set; }
        public String Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime Deleted { get; set; }
        public Boolean IsActive { get; set; }
    }
}
