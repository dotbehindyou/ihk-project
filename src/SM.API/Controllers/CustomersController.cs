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
        [HttpPut("{id}")]
        public Customer Put(Int32 id)
        {
            return cm.Create(id);
        }

        // DELETE: api/ApiWithActions/{Int32}
        [HttpDelete("{id}")]
        public void Delete(Int32 id)
        {
            cm.Remove(id);
        }

        #endregion

    }
}
