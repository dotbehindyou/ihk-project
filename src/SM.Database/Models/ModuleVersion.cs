using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Models
{
    public class ModuleVersion
    {
        public Guid Module_ID { get; set; }
        public String ModuleName { get; set; }
        public String Version { get; set; }
        public ConfigFile Config { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Byte[] ValidationToken { get; set; }
        public Byte[] File { get; set; }
        public String Status { get; set; }
    }
}
