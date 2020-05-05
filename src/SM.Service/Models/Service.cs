using SM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SM.Service.Models
{
    internal class Service
    {
        public Module Module { get; set; }
        public String Name { get; set; }
        public ServiceControllerStatus Status { get; set; }
        public String Path { get; set; }
    }
}
