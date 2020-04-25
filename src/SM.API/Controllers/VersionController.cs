using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SM.API.Managers;
using SM.Models;

namespace SM.API.Controllers
{
    [Route("api/v1/{module_id:Guid}/[controller]")]
    [ApiController]
    public class VersionController : BaseController
    {
        private ModuleManager mm;

        public VersionController(IOptions<Config> appSettings)
            : base(appSettings)
        {
            mm = new ModuleManager(Config.ConnectionString);
        }

        #region Verwaltung

        // GET: api/Version
        [HttpGet]
        public IEnumerable<ModuleVersion> Get(Guid module_id)
        {
            return mm.GetModuleVersions(module_id);
        }

        // GET: api/Version/5
        [HttpGet("{version}", Name = "Get")]
        public ModuleVersion Get(Guid module_id, String version)
        {
            return mm.GetVersion(module_id, version);
        }

        // POST: api/Version
        [HttpPost]
        public ModuleVersion Post(Guid module_id, [FromBody] ModuleVersion version)
        {
            // TODO File
            return mm.AddVersion(module_id, version.Version, version.File, version.ReleaseDate);
        }

        // PUT: api/Version/5
        [HttpPut("{id}")]
        public void Put(Guid module_id, [FromBody] ModuleVersion version)
        {
            // TODO File
            if(version.File != null)
                mm.UpdateVersion(module_id, version.Version, version.File);

            if (version.Config != null)
            {
                if (version.Config.Data == null && version.Config.Config_ID != Guid.Empty)
                    mm.SetConfig(module_id, version.Version, version.Config.Config_ID);
                else
                    mm.UpdateConfig(version.Config);
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{version}")]
        public void Delete(Guid module_id, String version)
        {
            mm.RemoveVersion(module_id, version);
        }

        #endregion

        #region ServiceAccess

        [HttpGet("{auth_token}")]
        public HttpResponseMessage Download(Guid module_id, Byte[] auth_token)
        {
            // TODO Download Version of Module
            Stream st = default(Stream);
            return FileResult("lol", "lol", st);
        }

        #endregion
    }
}
