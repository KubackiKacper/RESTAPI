using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RESTAPI_Backend.DTOs;
using RESTAPI_Backend.Enums;
using RESTAPI_Backend.Services;

namespace RESTAPI_Backend.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IToDoService _service;
        public ToDoController (IToDoService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("todo")]
        public async Task<IActionResult> Get()
        {
            var response = await _service
                .GetAll();
            return Ok(response);
        }

        [HttpPost]
        [Route("todo")]
        public async Task<IActionResult> Add([FromBody] SaveToDoDTO saveToDoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _service
                .AddToDo(saveToDoDTO);
            if (response == null)
            {
                return BadRequest("Could not add item");
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("todo/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var response = await _service
                .DeleteToDo(id);
            return Ok(response);
        }

        [HttpPut]
        [Route("todo/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] SaveToDoDTO toDoDto)
        {
            var response = await _service
                .UpdateToDo(id, toDoDto);
            if (response == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(response);
        }

        [HttpPut("todo/percent/{id}")]
        public async Task<IActionResult> SetPercentComplete([FromRoute] int id, [FromQuery] int percentComplete)
        {
            var response = await _service
                .SetToDoPercentComplete(id, percentComplete);

            if (response == null)
            {
                return BadRequest(ModelState);
            }
            return Ok(response);
        }

        [HttpPut("todo/done/{id}")]
        public async Task<IActionResult> MarkAsDone([FromRoute] int id, [FromQuery] bool isDone)
        {
            var response = await _service
                .MarkToDoAsDone(id, isDone);

            if (response == null)
            {
                return BadRequest(ModelState);
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("todo/{id}")]
        public async Task <IActionResult> GetById([FromRoute] int id)
        {
            var response = await _service
                .GetSpecificToDo(id);
            if (response == null)
            {
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(response);
        }

        [HttpGet("todo/incoming")]
        public async Task<IActionResult> GetIncomingToDos([FromQuery] DateFilter filter)
        {
            var response = await _service
                .GetIncomingToDos(filter);
            if (response == null || !response.Any())
            {
                return NotFound("No ToDos found for the given filter");
            }
            return Ok(response);
        }
    }
}
