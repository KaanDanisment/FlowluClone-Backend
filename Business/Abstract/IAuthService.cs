using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.jwt;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuthService
    {
        IDataResult<User> Register(UserRegisterDto userRegisterDto, string Password);
        IDataResult<User> Login(UserLoginDto userLoginDto);
        IResult UserExist(string Message);
        IDataResult<AccessToken> CreateAccessToken(User user);
    }
}
