using Core.DataAccess.Concrete.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Task = Entities.Concrete.Task;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfTaskDal : EfEntityRepositoryBase<Task,FlowluContext>, ITaskDal
    {
        
    }
}
