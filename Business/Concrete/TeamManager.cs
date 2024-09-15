using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class TeamManager : ITeamService
    {
        private ITeamDal _teamDal;
        private ITeamMemberDal _teamMemberDal;
        private IUserDal _userDal;
        private IHttpContextAccessor _httpContextAccessor;

        public TeamManager(ITeamDal teamDal, IHttpContextAccessor httpContextAccessor, ITeamMemberDal teamMemberDal, IUserDal userDal)
        {
            _teamDal = teamDal;
            _httpContextAccessor = httpContextAccessor;
            _teamMemberDal = teamMemberDal;
            _userDal = userDal;
        }

        public IDataResult<Team> CreateTeam()
        {
            var team = new Team();
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            team.UserId = userId;
            _teamDal.Add(team);

            var teamMember = new TeamMember
            {
                TeamId = team.Id,
                UserEmail = _userDal.Get(u => u.Id == userId).Email, 
                Role = "Admin"
            };
            _teamMemberDal.Add(teamMember);

            using (var context = new FlowluContext())
            {
                var oldOperationClaim = context.UserOperationClaims.FirstOrDefault(uoc => uoc.UserId == userId);
                if (oldOperationClaim != null)
                {
                    context.UserOperationClaims.Remove(oldOperationClaim);
                }
                var adminRole = context.OperationClaims.FirstOrDefault(oc => oc.Name == "Admin");
                if (adminRole != null)
                {
                    var userOperationClaim = new UserOperationClaim
                    {
                        UserId = userId,
                        OperationClaimId = adminRole.Id
                    };
                    context.UserOperationClaims.Add(userOperationClaim);
                    context.SaveChanges();
                }
            }

            return new SuccessDataResult<Team>(team);
        }




        public IResult DeleteTeam(int teamId)
        {
            var team = _teamDal.Get(t => t.Id == teamId);
            if(team == null)
            {
                return new ErrorResult(Messages.TeamNotFound);
            }
            var teamMembers = _teamMemberDal.GetList(t => t.TeamId == teamId).ToList();
            if(teamMembers.Count != 0)
            {
                using (var context = new FlowluContext())
                {
                    foreach (var teamMember in teamMembers)
                    {
                        var user = _userDal.Get(u => u.Email == teamMember.UserEmail);
                        if (user != null)
                        {
                            var userClaims = context.UserOperationClaims
                                                     .Where(uoc => uoc.UserId == user.Id)
                                                     .ToList();

                            if (userClaims.Any())
                            {
                                context.UserOperationClaims.RemoveRange(userClaims);
                            }
                        }

                        _teamMemberDal.Delete(teamMember);
                    }

                    context.SaveChanges();
                }
            }
            _teamDal.Delete(team);
            return new SuccessResult(Messages.TeamDeleted);
        }

        public IDataResult<Team> GetTeam()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _userDal.Get(u => u.Id == userId);
            var team = _teamDal.Get(t => t.UserId == userId);
            if (team == null)
            {
                var teamIsMemberOf = _teamMemberDal.Get(tm => tm.UserEmail == user.Email);
                if (teamIsMemberOf != null)
                {
                    team = _teamDal.Get(t => t.Id == teamIsMemberOf.TeamId);
                }
            }
            return new SuccessDataResult<Team>(team);
        }

        public IResult AddMemberToTeam(TeamMember teamMember)
        {
            var user = _userDal.Get(u => u.Email == teamMember.UserEmail);
            if (user == null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }
            var userHaveATeam = _teamDal.Get(t => t.UserId == user.Id);
            if (userHaveATeam != null)
            {
                return new ErrorResult(Messages.UserHaveATeam);
            }
            var existingUser = _teamMemberDal.Get(tm => tm.TeamId == teamMember.TeamId && tm.UserEmail == teamMember.UserEmail);
            if (existingUser != null)
            {
                return new ErrorResult(Messages.ExistingUser);
            }
            var userInAnotherTeam = _teamMemberDal.Get(tm => tm.UserEmail == teamMember.UserEmail);
            if (userInAnotherTeam != null)
            {
                return new ErrorResult(Messages.UserAlreadyInAnotherTeam);
            }
            using (var context = new FlowluContext())
            {
                var existingRole = context.UserOperationClaims.FirstOrDefault(uoc => uoc.UserId == user.Id && uoc.OperationClaimId == 4);
                if (existingRole != null)
                {
                    context.UserOperationClaims.Remove(existingRole);
                }

                var operationClaim = context.OperationClaims.FirstOrDefault(oc => oc.Name == teamMember.Role);
                if (operationClaim == null)
                {
                    return new ErrorResult(Messages.RoleNotFound);
                }

                var userOperationClaim = new UserOperationClaim
                {
                    UserId = user.Id,
                    OperationClaimId = operationClaim.Id
                };

                context.UserOperationClaims.Add(userOperationClaim);
                context.SaveChanges();
            }

            _teamMemberDal.Add(teamMember);

            return new SuccessResult(Messages.UsersAdded);
        }
        public IResult RemoveMember(string userEmail)
        {
            var teamMember = _teamMemberDal.Get(tm => tm.UserEmail == userEmail);
            if (teamMember == null)
            {
                return new ErrorResult(Messages.UserNotFoundTeam);
            }
            using (var context = new FlowluContext())
            {
                var user = _userDal.Get(u => u.Email == userEmail);
                if (user == null)
                {
                    return new ErrorResult(Messages.UserNotFound);
                }

                var userClaims = context.UserOperationClaims
                                         .Where(uoc => uoc.UserId == user.Id)
                                         .ToList();

                if (userClaims.Any())
                {
                    context.UserOperationClaims.RemoveRange(userClaims);
                    context.SaveChanges(); 
                }
            }
            _teamMemberDal.Delete(teamMember);
            return new SuccessResult(Messages.UserRemoved);
        }
        public IDataResult<List<TeamMemberDto>> GetTeamMembers()
        {
            var userList = new List<User>();
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _userDal.Get(u => u.Id == userId);
            var team = _teamDal.Get(t => t.UserId == userId);
            if (team == null)
            {
                var teamIsAMemberOf = _teamMemberDal.Get(tm => tm.UserEmail == user.Email);
                if (teamIsAMemberOf == null)
                {
                    return new ErrorDataResult<List<TeamMemberDto>>(Messages.TeamNotFound);
                }
                else
                {
                    team = _teamDal.Get(t => t.Id == teamIsAMemberOf.TeamId);
                }
            }
            var teamMembersEmail = _teamMemberDal.GetList(tm => tm.TeamId == team.Id).Select(tm => tm.UserEmail).ToList();
            foreach (var memberEmail in teamMembersEmail)
            {
                userList.Add(_userDal.Get(u => u.Email == memberEmail));
            }
            var teamMemberDtoList = new List<TeamMemberDto>();
            for(var i = 0; i < userList.Count; i++)
            {
                teamMemberDtoList.Add(new TeamMemberDto
                {
                    FirstName = userList[i].FirstName,
                    LastName = userList[i].LastName,
                    Email = userList[i].Email,
                    Role = _teamMemberDal.Get(tm => tm.UserEmail == userList[i].Email).Role
                });
            }
            return new SuccessDataResult<List<TeamMemberDto>>(teamMemberDtoList);
        }
    }
}
