using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Service.Models
{
    class ServiceNotInstalledException : Exception
    {
        private const String message = "Service is not Installed!";

        public Service Service { get; private set; }

        public ServiceNotInstalledException(Service service)
            : base(message)
        {
            this.Service = service;
        }
    }
}
