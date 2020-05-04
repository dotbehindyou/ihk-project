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
            mm = new ModuleManager(Config.ConnectionString);
        }

        // GET: api/Module
        [HttpGet]
        public IEnumerable<Module> Get()
        {
            return mm.GetMany();
        }

    }
}
