using System;
using System.Collections.Generic;
using System.Text;

namespace SM
{
    public class Config
    {
        public static Config Current { get; set; }

        public String ConnectionString { get; set; }
        public String Werk { get; set; }
        public String FileStore { get; set; }
    }
}
