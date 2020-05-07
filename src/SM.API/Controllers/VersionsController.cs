using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SM.Managers;
using SM.Models;

namespace SM.API.Controllers
{
    [Route("{module_id}/[controller]")]
    [ApiController]
    public class VersionsController : BaseController
    {

        [HttpGet("dl", Name = "Download")]
        public FileStreamResult Download([FromHeader] String auth_token, Guid module_id)
        {
            using(ModuleManager mm = new ModuleManager())
            using (CustomerManager cm = new CustomerManager(mm))
            {
                try
                {
                    Int32 kdnr = cm.GetCustomerKdnr(this.GetAuthToken(auth_token));
                    Module module = null;
                    return File(mm.GetVersionFile(module_id, kdnr, out module), "application/zip", $"{module.Name}.zip");
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
