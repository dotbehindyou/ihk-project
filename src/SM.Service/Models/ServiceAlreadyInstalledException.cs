using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Service.Models
{
    class ServiceAlreadyInstalledException : Exception
    {
        private const String message = "Service is already installed!";

        public Service Service { get; private set; }

        public ServiceAlreadyInstalledException(Service service)
            : base(message)
        {
            this.Service = service;
        }
    }
}