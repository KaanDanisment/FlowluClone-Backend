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
    public class TeamsController : ControllerBase
    {
        private ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost("createteam")]
        public IActionResult CreateTeam()
        {
            var result = _teamService.CreateTeam();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpGet("getteam")]
        public IActionResult GetTeam()
        {
            var result = _teamService.GetTeam();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(new { message = result.Message });
        }
        [HttpPost("deleteteam/{id}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult DeleteTeam(int id)
        {
            var result = _teamService.DeleteTeam(id);
            if (result.Success)
            {
                return Ok(new { message = result.Message });
            }
            return BadRequest(new { message = result.Message });
        }

        [HttpPost("addmember")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult AddMember([FromBody] TeamMember teamMember)
        {
            var result = _teamService.AddMemberToTeam(teamMember);
            if (result.Success)
            {
                return Ok(new { message = result.Message });
            }
            return BadRequest(new { message = result.Message });
        }

        [HttpPost("removemember/{userEmail}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult RemoveMember(string userEmail)
        {
            var result = _teamService.RemoveMember(userEmail);
            if(result.Success)
            {
                return Ok(new {message = result.Message});
            }
            return BadRequest(new { message = result.Message });
        }

        [HttpGet("getteammembers")]
        public IActionResult GetTeamMembers()
        {
            var result = _teamService.GetTeamMembers();
            if(result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
    }
}
