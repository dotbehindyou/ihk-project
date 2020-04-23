using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Models
{
    public class ConfigFile
    {
        public Guid Config_ID { get; set; }
        public String FileName { get; set; }
        public String Format { get; set; }
        public String Data { get; set; }
    }
}
