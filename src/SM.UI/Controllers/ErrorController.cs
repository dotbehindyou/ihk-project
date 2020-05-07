using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SM.UI.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase {
        [Route("/error")]
        public IActionResult Error() => Problem();
    }
}