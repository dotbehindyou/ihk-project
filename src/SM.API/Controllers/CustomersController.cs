using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SM.API.Managers;
using SM.Models;

namespace SM.API.Controllers
{
    //[Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CustomersController : BaseController
    {
        private CustomerManager cm;

        public CustomersController(IOptions<Config> appSettings)
            : base(appSettings)
        {
            cm = new CustomerManager(Config.ConnectionString, Config.Werk);
        }

        #region Verwaltung

        // GET: api/Customer
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return cm.GetMany();
        }

        // GET: api/Customer
        [HttpGet("Search")]
        public IEnumerable<Customer> Search([FromQuery] Search search)
        {
            return cm.GetMany(search);
        }

        // GET: api/Customer/{Int32}
        [HttpGet("{id}", Name = "Customer")]
        public Customer Get(Int32 id)
        {
            return cm.Get(id);
        }

        // PUT: api/Customer/{Int32}
        [HttpPut("{kdnr}")]
        public void Put(Int32 id)
        {
            cm.Create(id);
        }

        // DELETE: api/ApiWithActions/{Int32}
        [HttpDelete("{id}")]
        public void Delete(Int32 id)
        {
            cm.Remove(id);
        }

        [HttpPost]
        public void Changes(Int32 id, [FromBody] IDictionary<Module, ChangeItemOperation> change)
        {
            cm.AddChange(id, change);
        }

        [HttpGet("{id}")]
        public List<Change> Changes(Int32 id)
        {
            return cm.GetCustomerChanges(id, true);
        }

        #endregion

        #region ServiceAccess

        // [AllowAnonymous]
        [HttpGet("sa/changes")]
        public List<Change> Changes([FromHeader] String auth_token)
        {
            Int32 kdnr = cm.GetCustomerKdnr(this.GetAuthToken(auth_token));
            return cm.GetCustomerChanges(kdnr);
        }

        // [AllowAnonymous]
        [HttpPut("sa/changes/{change_id}")]
        public void Changes([FromHeader] String auth_token, [FromBody] Change change) // TODO Change ID
        {
            Int32 kdnr = cm.GetCustomerKdnr(this.GetAuthToken(auth_token));
            change.Kdnr = kdnr;

            cm.SetChange(change);
        }

        [HttpPut("sa/modules/{module_id}")]
        public void ModuleStatus([FromHeader] String auth_token, Guid module_id, ModuleStatus status)
        {
            Int32 kdnr = cm.GetCustomerKdnr(this.GetAuthToken(auth_token));

            cm.SetModuleStatus(kdnr, module_id, status);
        }

        [HttpDelete("sa/modules/{module_id}")]
        public void ModuleRemoved([FromHeader] String auth_token, Guid module_id)
        {
            Int32 kdnr = cm.GetCustomerKdnr(this.GetAuthToken(auth_token));

            cm.RemovedModuleFromCustomer(kdnr, module_id);
        }

        #endregion
    }
}
