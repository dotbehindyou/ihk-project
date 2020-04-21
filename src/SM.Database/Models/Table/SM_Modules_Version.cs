using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Models.Table
{
    public class SM_Modules_Version
    {
        public String Version { get; set; }
        public Guid Module_ID { get; set; }
        public String Validation_Token { get; set; }
        public Guid Config_ID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Deleted { get; set; }
        public Boolean IsActive { get; set; }
    }
}
