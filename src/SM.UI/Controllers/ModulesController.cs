using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SM.Managers;
using SM.Models;

namespace SM.API.Controllers
{
    [Route("api/[controller]")]
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
        public IEnumerable<Module> Get()
        {
            return mm.GetMany();
        }

        // GET: api/Module/Customer
        [HttpGet("Customer/{kdnr}")]
        public IEnumerable<Module> GetModulesFromCustomer(Int32 kdnr)
        {
            return mm.GetModulesFromCustomer(kdnr);
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

        #region CustomerModule

        [HttpPost("Customer/{kdnr}")]
        public Boolean AddModuleToCustomer(Int32 kdnr, [FromBody] ModuleVersion module)
        {
            mm.AddModuleToCustomer(kdnr, module);
            return true;
        }

        [HttpPut("Customer/{kdnr}")]
        public Boolean SetModuleToCustomer(Int32 kdnr, [FromBody] ModuleVersion module)
        {
            mm.SetModuleToCustomer(kdnr, module);
            return true;
        }

        [HttpDelete("Customer/{kdnr}")]
        public Boolean RemoveModuleToCustomer(Int32 kdnr, [FromBody] ModuleVersion module)
        {
            mm.RemoveModuleFromCustomer(kdnr, module);
            return true;
        }

        #endregion
    }
}
