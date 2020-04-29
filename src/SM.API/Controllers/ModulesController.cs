using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SM.API.Managers;
using SM.Models;

namespace SM.API.Controllers
{
    [Route("api/v1/[controller]")]
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
            return mm.GetAll();
        }

        // GET: api/Module/5
        [HttpGet("{id}", Name = "Module")]
        public Module Get(Guid id)
        {
            return mm.Get(id);
        }

        // Create: api/Modules
        [HttpPost]
        public Module Post([FromBody] Module value)
        {
            return mm.Create(value.Name);
        }

        // Set: api/Module/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] Module value)
        {
            mm.Update(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            mm.Remove(id);
        }
    }
}
