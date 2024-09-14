using Core.Utilities.Results;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IUserService
    {
        List<OperationClaim> GetClaims(User user);
        List<User> GetAllUsers();
        User GetUserByEmail(string email);
        void Add(User user);
        void Update(User user);
        void Delete(User user);
    }
}
