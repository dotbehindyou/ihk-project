using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SM.Managers;
using SM.Models;

namespace SM.API.Controllers
{
    [Route("api/{module_id}/[controller]")]
    [ApiController]
    public class VersionsController : BaseController
    {
        private ModuleManager mm;

        public VersionsController(IOptions<Config> appSettings)
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
        [HttpGet("{version}", Name = "Version")]
        public ModuleVersion Get(Guid module_id, String version)
        {
            return mm.GetVersion(module_id, version);
        }

        // GET: api/Version/5
        [HttpGet("{version}/{kdnr}", Name = "VersionFromKdnr")]
        public ModuleVersion GetVersionFromCustomer(Guid module_id, String version, Int32 kdnr)
        {
            return mm.GetVersionFromCustomer(module_id, version, kdnr);
        }

        // POST: api/Version
        [HttpPost]
        public ModuleVersion Post(Guid module_id, [FromBody] ModuleVersion version)
        {
            return mm.AddVersion(module_id, version.Version, version.Config, version.ReleaseDate);
        }

        [HttpPut("{version}/file")]
        public Boolean UploadFile(Guid module_id, String version)
        {
            var httpRequest = HttpContext.Request;
            if (httpRequest.Form.Files.Count > 0)
            {
                var file = httpRequest.Form.Files[0];
                mm.UpdateVersionFiles(module_id, version, file.OpenReadStream());
            }
            // TODO File

            return false;
        }

        // PUT: api/Version/5
        [HttpPut("{version}")]
        public ModuleVersion Put(Guid module_id, String version, [FromBody] ModuleVersion versionForm)
        {
            if (versionForm.Config != null)
            {
                if (versionForm.Config.Config_ID == Guid.Empty)
                {
                    versionForm.Config = mm.CreateConfig(module_id, versionForm.Config.FileName, versionForm.Config.Format, versionForm.Config.Data);
                    mm.SetConfig(module_id, version, versionForm.Config.Config_ID);
                }
                else if (versionForm.Config.Data == null && versionForm.Config.Config_ID != Guid.Empty)
                    mm.SetConfig(module_id, versionForm.Version, versionForm.Config.Config_ID);
                else
                    mm.UpdateConfig(versionForm.Config);
            }

            return versionForm;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{version}")]
        public void Delete(Guid module_id, String version)
        {
            mm.RemoveVersion(module_id, version);
        }

        #endregion
    }
}
