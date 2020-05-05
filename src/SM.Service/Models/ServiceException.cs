using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM.Service.Models
{
    class ServiceException : Exception
    {
        public Service Service { get; private set; }

        public ServiceException(Service service, String message)
            : base(message)
        {
            this.Service = service;
        }
    }
}