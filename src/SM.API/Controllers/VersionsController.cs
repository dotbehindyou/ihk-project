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
        private ModuleManager mm;

        public VersionsController(IOptions<Config> appSettings)
            : base(appSettings)
        {
            mm = new ModuleManager(Config.FileStore, Config.ConnectionString);
        }

        [HttpGet("dl", Name = "Download")]
        public FileStreamResult Download([FromHeader] String auth_token, Guid module_id)
        {
            Int32 kdnr;
            Module module = null;
            using(CustomerManager cm = new CustomerManager(this.Config.ConnectionString, this.Config.Werk))
                kdnr = cm.GetCustomerKdnr(this.GetAuthToken(auth_token));
            return File(mm.GetVersionFile(module_id, kdnr, out module), "application/zip", $"{module.Name}.zip");
        }
    }
}
