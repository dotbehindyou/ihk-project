using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SM.Managers;
using SM.Models;

namespace SM.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ModulesController : BaseController
    {
        private ModuleManager mm;

        public ModulesController(IOptions<Config> appSettings)
            : base(appSettings)
        {
            mm = new ModuleManager(Config.FileStore, Config.ConnectionString);
        }

        // GET: api/Module
        [HttpGet]
        public IEnumerable<Module> Get([FromHeader] String auth_Token)
        {
            Int32 kdnr;
            using (CustomerManager cm = new CustomerManager(this.Config.ConnectionString, this.Config.Werk))
                kdnr = cm.GetCustomerKdnr(this.GetAuthToken(auth_Token));

            return mm.GetModulesFromCustomer(kdnr);
        }

    }
}
