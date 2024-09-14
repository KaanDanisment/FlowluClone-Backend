using Core.Entities.Concrete;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ITeamService
    {
        IDataResult<Team> CreateTeam();
        IDataResult<Team> GetTeam();
        IResult DeleteTeam(int teamId);
        IResult AddMemberToTeam(TeamMember teamMember);
        IResult RemoveMember(string userEmail);
        IDataResult<List<TeamMemberDto>> GetTeamMembers();
    }
}
