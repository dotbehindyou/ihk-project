using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Models.Table
{
    public class SM_Modules_Config
    {
        public Guid Config_ID { get; set; }
        public Guid Module_ID { get; set; }
        public String FileName { get; set; }
        public String Format { get; set; }
        public String Data { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Deleted { get; set; }
        public Boolean IsActive { get; set; }
    }
}
