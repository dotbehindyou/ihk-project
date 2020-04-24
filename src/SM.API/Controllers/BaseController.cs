using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SM.API.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IOptions<Config> config;

        protected Config Config { get => config?.Value; }

        public BaseController(IOptions<Config> appSettings)
        {
            this.config = appSettings;
        }
    }
}
