﻿using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace Business.Concrete
{
    public class ProjectManager : IProjectService
    {
        private IProjectDal _projectDal;
        private ICustomerDal _customerDal;
        private ITaskDal _taskDal;
        private ITeamMemberDal _teamMemberDal;
        private ITeamDal _teamDal;
        private IUserDal _userDal;
        private IHttpContextAccessor _httpContextAccessor;
        private IMapper _mapper;

        public ProjectManager(IProjectDal projectDal, ICustomerDal customerDal, IHttpContextAccessor httpContextAccessor, IMapper mapper, ITaskDal taskDal, ITeamMemberDal teamMemberDal, ITeamDal teamDal, IUserDal userDal)
        {
            _projectDal = projectDal;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _customerDal = customerDal;
            _taskDal = taskDal;
            _teamMemberDal = teamMemberDal;
            _teamDal = teamDal;
            _userDal = userDal;
        }

        public IDataResult<Project> CreateProject(Project project)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _userDal.Get(u => u.Id == userId);
            var teamMember = _teamMemberDal.Get(tm => tm.UserEmail == user.Email);
            if(teamMember == null)
            {
                project.UserId = userId;
            }
            else
            {
                var team = _teamDal.Get(t => t.Id == teamMember.TeamId);
                project.UserId = team.UserId;
            }


            _projectDal.Add(project);
            return new SuccessDataResult<Project>(project,Messages.ProjectAdded);
        }

        public IResult Delete(int projectId)
        {
            var project = _projectDal.Get(p=>p.Id == projectId);
            if(project == null)
            {
                return new ErrorResult(Messages.ProjectNotFound);
            }
            var tasks = _taskDal.GetList(t => t.ProjectId == projectId).ToList();
            if(tasks.Count != 0)
            {
                tasks.ForEach(t => _taskDal.Delete(t));
            }

            _projectDal.Delete(project);
            return new SuccessResult(Messages.ProjectDeleted);
        }

        public IDataResult<List<ProjectDetailDto>> GetProjects()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var user = _userDal.Get(u => u.Id == userId);
            var userTeam = _teamMemberDal.Get(tm => tm.UserEmail == user.Email);
            var projects = new List<Project>();
            if(userTeam != null)
            {
                var team = _teamDal.Get(t => t.Id == userTeam.TeamId);
                projects = _projectDal.GetList(p => p.UserId == team.UserId).ToList();
            }
            else
            {
                projects = _projectDal.GetList(p => p.UserId == userId).ToList();

            }
            var projectsToReturn = _mapper.Map<List<ProjectDetailDto>>(projects);

            foreach(var project in projectsToReturn)
            {
                if(project.CustomerId != 0)
                {
                    project.CustomerName = _customerDal.Get(c => c.Id == project.CustomerId).Name;
                }
            }

            return new SuccessDataResult<List<ProjectDetailDto>>(projectsToReturn);

        }
        public IDataResult<ProjectDetailDto> GetProject(int projectId)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var project = _projectDal.Get(p => p.Id == projectId && p.UserId == userId);
            var projectToReturn = _mapper.Map<ProjectDetailDto>(project);
            if(project.CustomerId != 0)
            {
                projectToReturn.CustomerName = _customerDal.Get(c => c.Id == projectToReturn.CustomerId).Name;
            }

            return new SuccessDataResult<ProjectDetailDto>(projectToReturn);
        }

        public IDataResult<List<ProjectDetailDto>> GetProjectsByCustomerId(int customerId)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var projects = _projectDal.GetList(p => p.CustomerId == customerId && p.UserId == userId);
            var projectsToReturn = _mapper.Map<List<ProjectDetailDto>>(projects);

            return new SuccessDataResult<List<ProjectDetailDto>>(projectsToReturn);
        }

        public IResult Update(Project project)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _userDal.Get(u => u.Id == userId);
            var teamMember = _teamMemberDal.Get(tm => tm.UserEmail == user.Email);
            var team = _teamDal.Get(t => t.Id == teamMember.TeamId);

            project.UserId = team.UserId;
            _projectDal.Update(project);
            return new SuccessResult(Messages.ProjectUpdated);
        }
    }
}