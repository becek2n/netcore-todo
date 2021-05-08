using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TODO.DTO;

namespace TODO.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;

        public TokenController(ILogger<TaskController> logger) {
            _logger = logger;
        }

        [HttpPost]
        public ActionResult Post() {
            return Ok();

        }

        [HttpPost("/Refresh")]
        public ActionResult Refresh(TokenRequestDTO model)
        {

            return Ok();
        }
    }
}
