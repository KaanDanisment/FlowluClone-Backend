using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Core.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Constants;
using DataAccess.Concrete.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Entities.Concrete;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private IUserDal _userDal;

        public  UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }
        public List<OperationClaim> GetClaims(User user)
        {
            return _userDal.GetClaims(user);
        }

        public void Add(User user)
        {
            _userDal.Add(user);
        }

        public void Delete(User user)
        {
            _userDal.Delete(user);
        }

        public List<User> GetAllUsers()
        {
            return _userDal.GetList();
        }

        public User GetUserByEmail(string email)
        {
            return _userDal.Get(u => u.Email==email);
        }

        public void Update(User user)
        {
            _userDal.Update(user);
        }
    }
}
