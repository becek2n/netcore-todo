﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TODO.DTO;
using TODO.Interfaces;

namespace TODO.API.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ILogger<TaskController> _logger;
        private readonly ITask _task;

        public TaskController(ILogger<TaskController> logger, ITask task)
        {
            _logger = logger;
            _task = task;
        }

        [HttpGet]
        public ActionResult Get([FromQuery] int index, [FromQuery] int size, [FromQuery] string search)
        {
            return Ok(_task.GetAll(index, size, search));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var x = await _task.GetId(id);
            if (id == 2) {
                return Unauthorized();
            }
            return Ok(x);
        }

        [HttpPost]
        public ActionResult Post(TaskDTO model)
        {
            return Ok(_task.Add(model));
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] TaskEditDTO model)
        {
            return Ok(_task.Update(id, model));
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok(_task.Delete(id));
        }
    }
}
