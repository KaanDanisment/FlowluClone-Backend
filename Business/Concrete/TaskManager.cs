﻿using Business.Abstract;
using DataAccess.Abstract;
using Task = Entities.Concrete.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Utilities.Results;
using Business.Constants;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using AutoMapper;
using Entities.Dtos;

namespace Business.Concrete
{
    public class TaskManager : ITaskService
    {
        private ITaskDal _taskDal;
        private IProjectDal _projectDal;
        private IHttpContextAccessor _httpContextAccessor;
        private IMapper _mapper;
        public TaskManager(ITaskDal taskDal, IHttpContextAccessor httpContextAccessor, IProjectDal projectDal,IMapper mapper)
        {
            _taskDal = taskDal;
            _httpContextAccessor = httpContextAccessor;
            _projectDal = projectDal;
            _mapper = mapper;
        }

        public IDataResult<Task> CreateTask(Task task)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            task.UserId = int.Parse(userId);
            _taskDal.Add(task);
            return new SuccessDataResult<Task>(task,Messages.TaskAdded);
        }

        public IResult Delete(int taskId)
        {
            var task = _taskDal.Get(t => t.Id == taskId);
            if(task == null)
            {
                return new ErrorResult(Messages.TaskNotFound);
            }
            _taskDal.Delete(task);
            return new SuccessResult(Messages.TaskDeleted);
        }

        public IDataResult<List<TaskDetailDto>> GetTasks()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var tasks = _taskDal.GetList(t => t.UserId == userId);
            var tasksToReturn = _mapper.Map<List<TaskDetailDto>>(tasks);
            foreach (var task in tasksToReturn)
            {
                if (task.ProjectId != 0)
                {
                    task.ProjectName = _projectDal.Get(p => p.Id == task.ProjectId).Name;
                }
            }

            return new SuccessDataResult<List<TaskDetailDto>>(tasksToReturn);
        }

        public IDataResult<TaskDetailDto> GetTask(int taskId)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var task = _taskDal.Get(t => t.Id == taskId && t.UserId == userId);
            var taskToReturn = _mapper.Map<TaskDetailDto>(task);
            if(task.ProjectId != 0)
            {
                taskToReturn.ProjectName = _projectDal.Get(p => p.Id == taskToReturn.ProjectId).Name;
            }

            return new SuccessDataResult<TaskDetailDto>(taskToReturn);
        }

        public IDataResult<List<TaskDetailDto>> GetTasksByProjectId(int projectId)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var tasks = _taskDal.GetList(t => t.ProjectId == projectId && t.UserId == userId);
            var tasksToReturn = _mapper.Map<List<TaskDetailDto>>(tasks);
            foreach (var task in tasksToReturn)
            {
                if (task.ProjectId != 0)
                {
                    task.ProjectName = _projectDal.Get(p => p.Id == task.ProjectId).Name;
                }
            }

            return new SuccessDataResult<List<TaskDetailDto>>(tasksToReturn);
        }

        public IResult Update(Task task)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            task.UserId = int.Parse(userId);
            if(task == null)
            {
                new ErrorResult(Messages.TaskNotFound);
            }
            _taskDal.Update(task);
            return new SuccessResult(Messages.TaskUpdated);
        }
    }
}
