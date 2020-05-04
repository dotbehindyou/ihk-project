using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SM.Managers;

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
            mm = new ModuleManager(Config.ConnectionString);
        }

        #region ServiceAccess

        [HttpGet("dl/{version}", Name = "Download")]
        public FileStreamResult Download(Guid module_id, String version, [FromHeader] String auth_token)
        {
            Byte[] auth_tokensd = this.GetAuthToken(auth_token);
            // TODO Auth_token Validieren und prüfen ob berechtigt ist auf Dateien zuzugreifen
            return File(mm.GetVersionFile(module_id, version), "application/zip", "ModulName.zip");
        }

        #endregion
    }
}
