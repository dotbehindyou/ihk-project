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
    public class VersionsController : ControllerBase {
        #region Verwaltung

        // GET: api/Version
        [HttpGet]
        public IEnumerable<ModuleVersion> Get(Guid module_id)
        {
            using (ModuleManager mm = new ModuleManager())
                try
                {
                    return mm.GetModuleVersions(module_id);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
        }

        // GET: api/Version/5
        [HttpGet("{version}", Name = "Version")]
        public ModuleVersion Get(Guid module_id, String version)
        {
            using (ModuleManager mm = new ModuleManager())
                try
                {
                    return mm.GetVersion(module_id, version);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
        }

        // GET: api/Version/5
        [HttpGet("{version}/{kdnr}", Name = "VersionFromKdnr")]
        public ModuleVersion GetVersionFromCustomer(Guid module_id, String version, Int32 kdnr)
        {
            using (ModuleManager mm = new ModuleManager())
                try
                {
                    return mm.GetVersionFromCustomer(module_id, version, kdnr);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
        }

        // POST: api/Version
        [HttpPost]
        public ModuleVersion Post(Guid module_id, [FromBody] ModuleVersion version)
        {
            using (ModuleManager mm = new ModuleManager())
                try
                {
                    return mm.AddVersion(module_id, version.Version, version.Config, version.ReleaseDate);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
        }

        [HttpPut("{version}/file")]
        public Boolean UploadFile(Guid module_id, String version)
        {
            var httpRequest = HttpContext.Request;
            if (httpRequest.Form.Files.Count > 0)
            {
                var file = httpRequest.Form.Files[0];
                using (ModuleManager mm = new ModuleManager())
                    try
                    {
                        mm.UpdateVersionFiles(module_id, version, file.OpenReadStream());
                    }
                    catch (Exception e)
                    {
                        mm.Rollback();
                        throw e;
                    }
            }
            // TODO File

            return true;
        }

        // PUT: api/Version/5
        [HttpPut("{version}")]
        public ModuleVersion Put(Guid module_id, String version, [FromBody] ModuleVersion versionForm)
        {
            using (ModuleManager mm = new ModuleManager())
                try
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
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
            return versionForm;
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{version}")]
        public void Delete(Guid module_id, String version)
        {
            using (ModuleManager mm = new ModuleManager())
                try
                {
                    mm.RemoveVersion(module_id, version);
                }
                catch (Exception e)
                {
                    mm.Rollback();
                    throw e;
                }
        }

        #endregion
    }
}
