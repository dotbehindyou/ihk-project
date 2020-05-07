using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Models.Table {
    public class SM_Modules_Installed {
        public Guid Module_ID { get; set; }
        public String ServiceName { get; set; }
        public String Version { get; set; }
        public Byte[] ValidationToken { get; set; }
        public String ModuleName { get; set; }
        public String Path { get; set; }
        public DateTime Installed { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Deleted { get; set; }
        public Boolean IsActive { get; set; }
    }
}
