using Autofac;
using AutoMapper;
using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Security.jwt;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DependencyResolvers.Autofac
{
    public class AutofacBusinessModule:Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CustomerManager>().As<ICustomerService>();
            builder.RegisterType<EfCustomerDal>().As<ICustomerDal>();

            builder.RegisterType<ProjectManager>().As<IProjectService>();
            builder.RegisterType<EfProjectDal>().As<IProjectDal>();

            builder.RegisterType<TaskManager>().As<ITaskService>();
            builder.RegisterType<EfTaskDal>().As<ITaskDal>();

            builder.RegisterType<UserManager>().As<IUserService>();
            builder.RegisterType<EfUserDal>().As<IUserDal>();

            builder.RegisterType<AuthManager>().As<IAuthService>();
            builder.RegisterType<JwtHelper>().As<ITokenHelper>();

            builder.RegisterType<TeamManager>().As<ITeamService>();
            builder.RegisterType<EfTeamDal>().As<ITeamDal>();
            builder.RegisterType<EfTeamMemberDal>().As<ITeamMemberDal>();

            builder.Register(context =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddMaps(typeof(AutofacBusinessModule).Assembly);
                });

                return config.CreateMapper();
            }).As<IMapper>().SingleInstance();
        }
    }
}
