using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.Abstractions.Services;
using Domain.Commands;
using Domain.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;

        public TasksController(ITasksService taskService)
        {
            _tasksService = taskService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(CreateTasksCommandResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Create(CreateTasksCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _tasksService.CreateTasksCommandHandler(command);

            return Created($"/api/tasks/{result.Payload.Id}", result);
        }
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UpdateTasksCommandResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Update(Guid id, UpdateTasksCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _tasksService.UpdateTasksCommandHandler(command);

                return Ok(result);
            }
            catch (NotFoundException<Guid>)
            {
                return NotFound();
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetAllTasksQueryResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _tasksService.GetAllTasksQueryHandler();

            return Ok(result);
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(DeleteTasksCommandResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _tasksService.DeleteTasksCommandHandler(id);

                return Ok(result);
            }
            catch (NotFoundException<Guid>)
            {
                return NotFound();
            }
        }

    }
}
