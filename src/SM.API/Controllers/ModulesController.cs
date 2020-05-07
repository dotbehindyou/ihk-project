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

        // GET: api/Module
        [HttpGet]
        public IEnumerable<Module> Get([FromHeader] String auth_Token)
        {
            using (ModuleManager mm = new ModuleManager())
            using (CustomerManager cm = new CustomerManager(mm))
            {
                try
                {
                    Int32 kdnr = cm.GetCustomerKdnr(this.GetAuthToken(auth_Token));

                    return mm.GetModulesForService(kdnr);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
            }
        }

        [HttpPut]
        public void SetStatus([FromHeader] String auth_Token, [FromBody] Module module)
        {
            using (ModuleManager mm = new ModuleManager())
            using (CustomerManager cm = new CustomerManager(mm))
            {
                try
                {
                    Int32 kdnr = cm.GetCustomerKdnr(this.GetAuthToken(auth_Token));
                    mm.SetModuleStatusFromCustomer(kdnr, module.Module_ID, module.Status);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
            }
        }

        [HttpDelete]
        public void Delete([FromHeader] String auth_Token, Guid module_id)
        {
            using (ModuleManager mm = new ModuleManager())
            using (CustomerManager cm = new CustomerManager(mm))
            {
                try
                {
                    Int32 kdnr = cm.GetCustomerKdnr(this.GetAuthToken(auth_Token));
                    mm.RemoveModuleFromCustomer(kdnr, module_id, true);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
            }
        } 
    }
}
