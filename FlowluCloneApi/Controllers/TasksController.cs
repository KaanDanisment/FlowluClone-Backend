using Business.Abstract;
using Task = Entities.Concrete.Task;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FlowluCloneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("gettasks")]
        public IActionResult GetTasks()
        {
            var result = _taskService.GetTasks();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
        [HttpGet("gettask/{id}")]
        public IActionResult GetTask(int id)
        {
            var result = _taskService.GetTask(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
        [HttpGet("gettasksbyprojectid/{projectId}")]
        public IActionResult GetTasksByProjectId(int projectId)
        {
            var result = _taskService.GetTasksByProjectId(projectId);
            if(result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("createtask")]
        public IActionResult CreateTask(Task task)
        {
            var result = _taskService.CreateTask(task);
            if (result.Success)
            {
                return Ok(new {task = result.Data, message = result.Message });
            }
            return BadRequest(result.Message);
        }

        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _taskService.Delete(id);
            if (result.Success)
            {
                return Ok(new {message = result.Message});
            }
            return BadRequest(result.Message);
        }

        [HttpPost("update")]
        public IActionResult update(Task task)
        {
            var result = _taskService.Update(task);
            if (result.Success)
            {
                return Ok(new { message = result.Message });
            }
            return BadRequest(result.Message);
        }
    }
}
