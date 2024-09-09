using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlowluCloneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        
        [HttpGet("getprojects")]
        public IActionResult GetProjects()
        {
            var result = _projectService.GetProjects();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getproject/{id}")]
        public IActionResult GetProject(int id)
        {
            var result = _projectService.GetProject(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getprojectbycustomerid/{id}")]
        public IActionResult GetProjectByCustomerId(int id)
        {
            var result = _projectService.GetProjectsByCustomerId(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("createproject")]
        public IActionResult CreateProject(Project project)
        {
            var result = _projectService.CreateProject(project);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("delete/{id}")]
        public IActionResult Delete(int id)
        {
            var result = _projectService.Delete(id);
            if (result.Success)
            {
                return Ok( new { message = result.Message});
            }
            return BadRequest(result.Message);
        }

        [HttpPost("update")]
        public IActionResult Update(Project project)
        {
            var result = _projectService.Update(project);
            if (result.Success)
            {
                return Ok( new { message = result.Message});
            }
            return BadRequest(result.Message);
        }
    }
}
