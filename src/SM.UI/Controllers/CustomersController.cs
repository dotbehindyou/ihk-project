using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SM.Managers;
using SM.Models;

namespace SM.API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase {

        #region Verwaltung

        // GET: api/Customer
        [HttpGet]
        public IEnumerable<Customer> Get()
        {
            using (CustomerManager cm = new CustomerManager())
            {
                try
                {
                    return cm.GetMany();
                }
                catch(Exception e)
                {
                    cm.Rollback();
                    throw e;
                }
            }
        }

        // GET: api/Customer
        [HttpGet("Search")]
        public IEnumerable<Customer> Search([FromQuery] Search search)
        {
            using (CustomerManager cm = new CustomerManager())
                try
                {
                    return cm.GetMany(search);
                }
                catch (Exception e)
                {
                    cm.Rollback();
                    throw e;
                }
        }

        // GET: api/Customer/{Int32}
        [HttpGet("{id}", Name = "Customer")]
        public Customer Get(Int32 id)
        {
            using (CustomerManager cm = new CustomerManager())
                try
                {
                    return cm.Get(id);
                }
                catch (Exception e)
                {
                    cm.Rollback();
                    throw e;
                }
        }

        // PUT: api/Customer/{Int32}
        [HttpPut("{id}")]
        public Customer Put(Int32 id)
        {
            using (CustomerManager cm = new CustomerManager())
                try
                {
                    return cm.Create(id);
                }
                catch (Exception e)
                {
                    cm.Rollback();
                    throw e;
                }
        }

        // DELETE: api/ApiWithActions/{Int32}
        [HttpDelete("{id}")]
        public void Delete(Int32 id)
        {
            using (CustomerManager cm = new CustomerManager())
                try
                {
                    cm.Remove(id);
                }
                catch (Exception e)
                {
                    cm.Rollback();
                    throw e;
                }
        }

        #endregion
    }
}
