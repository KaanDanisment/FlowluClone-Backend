using AutoMapper;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Entities.Concrete.Task;

namespace Business.Helpers
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles() {
            CreateMap<Project, ProjectDetailDto>();
            CreateMap<Task, TaskDetailDto>();
        }
    }
}
