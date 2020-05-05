using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Models.Procedure
{
    public class Modules_Version_Config
    {
        public Guid Module_ID { get; set; }
        public String ModuleName { get; set; }
        public String Version { get; set; }
        public Guid Config_ID { get; set; }
        public String ConfigFileName { get; set; }
        public String ConfigFormat { get; set; }
        public String ConfigData { get; set; }
        public DateTime Release_Date { get; set; }
        public Byte[] Validation_Token { get; set; }
        public String Status { get; set; }
    }
}
