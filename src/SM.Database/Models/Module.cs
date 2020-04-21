using System;
using System.Collections.Generic;
using System.Text;

namespace SM.Models
{
    public class Module
    {
        public Guid Module_ID { get; set; }
        public String Name { get; set; }
        public String Version { get; set; }
        public ConfigFile Config { get; set; }
        public String GetFullName()
        {
            return System.IO.Path.Combine(Name, Version + ".zip");
        }
    }
}
