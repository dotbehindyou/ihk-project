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
        [HttpGet("{id}", Name = "Select")]
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
        #endregion


        [HttpGet("{id}")]
        public List<Change> GetChanges(Guid id)
        {
            return cm.GetCustomerChanges(id, true);
        }

        [HttpPost]
        public void AddChange(Customer customer)
        {
            customer.
        }

        // [AllowAnonymous]
        [HttpGet("{auth_token}")]
        public List<Change> GetChanges(Byte[] auth_token)
        {
            Guid customer_id = cm.GetCustomerId(auth_token);
            return cm.GetCustomerChanges(customer_id);
        }

        // [AllowAnonymous]
        [HttpPut("{auth_token}")]
        public void SetChange(Byte[] auth_token, Change change)
        {
            Guid customer_id = cm.GetCustomerId(auth_token);
            change.Customer_ID = customer_id;

            cm.SetChange(change);
        }
    }
}
