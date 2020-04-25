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
    public class CustomerController : BaseController
    {
        private CustomerManager cm;

        public CustomerController(IOptions<Config> appSettings)
            : base(appSettings)
        {
            cm = new CustomerManager(Config.ConnectionString, Config.Werk);
        }

        #region Verwaltung

        // GET: api/Customer
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            return cm.GetAll();
        }

        // GET: api/Customer/5
        [HttpGet("{id}", Name = "Customer")]
        public Customer Get(Guid id)
        {
            return cm.Get(id);
        }

        // PUT: api/Customer/5
        [HttpPut("{kdnr}")]
        public void Put(Int32 kdnr)
        {
            cm.Create(kdnr);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
            cm.Remove(id);
        }

        [HttpPost]
        public void AddChange(Guid customer_id, [FromBody] IDictionary<Module, ChangeItemOperation> change)
        {
            cm.AddChange(customer_id, change);
        }

        [HttpGet("{id}")]
        public List<Change> Changes(Guid id)
        {
            return cm.GetCustomerChanges(id, true);
        }

        #endregion

        #region ServiceAccess

        // [AllowAnonymous]
        [HttpGet("sa")]
        public List<Change> Changes([FromHeader] String auth_token)
        {
            Guid customer_id = cm.GetCustomerId(this.GetAuthToken(auth_token));
            return cm.GetCustomerChanges(customer_id);
        }

        // [AllowAnonymous]
        [HttpPut("sa")]
        public void Change([FromHeader] String auth_token, [FromBody] Change change)
        {
            Guid customer_id = cm.GetCustomerId(this.GetAuthToken(auth_token));
            change.Customer_ID = customer_id;

            cm.SetChange(change);
        }

        [HttpPut("sa/{module_id}")]
        public void ModuleStatus([FromHeader] String auth_token, Guid module_id, ModuleStatus status)
        {
            Guid customer_id = cm.GetCustomerId(this.GetAuthToken(auth_token));

            cm.SetModuleStatus(customer_id, module_id, status);
        }

        [HttpDelete("sa/{module_id}")]
        public void ModuleRemoved([FromHeader] String auth_token, Guid module_id)
        {
            Guid customer_id = cm.GetCustomerId(this.GetAuthToken(auth_token));

            cm.RemovedModuleFromCustomer(customer_id, module_id);
        }

        #endregion
    }
}
