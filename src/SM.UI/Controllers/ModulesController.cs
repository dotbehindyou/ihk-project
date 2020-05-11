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
    public class ModulesController : ControllerBase { 

        // GET: api/Module
        [HttpGet]
        public IEnumerable<Module> Get()
        {
            using (ModuleManager mm = new ModuleManager())
                try
                {
                    return mm.GetMany();
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
        }

        // GET: api/Module/Customer
        [HttpGet("Customer/{kdnr}")]
        public IEnumerable<Module> GetModulesFromCustomer(Int32 kdnr)
        {
            using (ModuleManager mm = new ModuleManager())
                return mm.GetModulesFromCustomer(kdnr);
        }

        // GET: api/Module/5
        [HttpGet("{id}", Name = "Module")]
        public Module Get(Guid id)
        {
            using (ModuleManager mm = new ModuleManager())
                try
                {
                    return mm.Get(id);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
        }

        // Create: api/Modules
        [HttpPost]
        public Module Post([FromBody] Module value)
        {
            using (ModuleManager mm = new ModuleManager())
                try
                {
                    return mm.Create(value.Name);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
        }

        // Set: api/Module/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] Module value)
        {
            using (ModuleManager mm = new ModuleManager())
                try
                {
                    mm.Update(value);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            using (ModuleManager mm = new ModuleManager())
                try
                {
                    mm.Remove(id);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
        }

        #region CustomerModule

        [HttpPost("Customer/{kdnr}")]
        public void AddModuleToCustomer(Int32 kdnr, [FromBody] ModuleVersion module)
        {
            using (ModuleManager mm = new ModuleManager())
                try
                {
                    mm.AddModuleToCustomer(kdnr, module);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
        }

        [HttpPut("Customer/{kdnr}")]
        public void SetModuleToCustomer(Int32 kdnr, [FromBody] ModuleVersion module)
        {
            using (ModuleManager mm = new ModuleManager())
                try
                {
                    mm.SetModuleToCustomer(kdnr, module);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
        }

        [HttpDelete("Customer/{kdnr}")]
        public void RemoveModuleToCustomer(Int32 kdnr, [FromBody] ModuleVersion module)
        {
            using (ModuleManager mm = new ModuleManager())
                try
                {
                    mm.RemoveModuleFromCustomer(kdnr, module.Module_ID, false);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
        }

        #endregion
    }
}
