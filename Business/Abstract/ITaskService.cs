using System;
using Task = Entities.Concrete.Task;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Utilities.Results;
using Entities.Dtos;

namespace Business.Abstract
{
    public interface ITaskService
    {
        IDataResult<Task> CreateTask(Task task);
        IDataResult<List<TaskDetailDto>> GetTasks();
        IDataResult<TaskDetailDto> GetTask(int taskId);
        IDataResult<List<TaskDetailDto>> GetTasksByProjectId(int projectId);
        IResult Update(Task task);
        IResult Delete(int taskid);
    }
}
