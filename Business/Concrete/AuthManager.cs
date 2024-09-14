using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.jwt;
using DataAccess.Concrete.EntityFramework.Context;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class AuthManager : IAuthService
    {
        private IUserService _userService;
        private ITokenHelper _tokenHelper;

        public AuthManager(IUserService userService, ITokenHelper tokenHelper)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
        }

        public IDataResult<AccessToken> CreateAccessToken(User user)
        {
            var claims = _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user,claims);
            return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreted);
        }

        public IDataResult<User> Login(UserLoginDto userLoginDto)
        {
            var userToCheck = _userService.GetUserByEmail(userLoginDto.Email);
            if (userToCheck == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }
            if(!HashingHelper.VerifyPassword(userLoginDto.Password,userToCheck.PasswordHash,userToCheck.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }
            using (var context = new FlowluContext())
            {
                // Kullanıcının herhangi bir rolü olup olmadığını kontrol et
                var userHasRole = context.UserOperationClaims.Any(uc => uc.UserId == userToCheck.Id);

                // Eğer rolü yoksa "User" rolü veriyoruz
                if (!userHasRole)
                {
                    var defaultRole = context.OperationClaims.FirstOrDefault(c => c.Name == "User");

                    if (defaultRole != null)
                    {
                        var userOperationClaim = new UserOperationClaim
                        {
                            UserId = userToCheck.Id,
                            OperationClaimId = defaultRole.Id
                        };

                        context.UserOperationClaims.Add(userOperationClaim); // Rol ataması
                        context.SaveChanges(); // Veritabanına kaydediyoruz
                    }
                    else
                    {
                        return new ErrorDataResult<User>("Varsayılan 'User' rolü bulunamadı.");
                    }
                }
            }

            return new SuccessDataResult<User>(userToCheck,Messages.SuccessfulLogin);
        }

        public IDataResult<User> Register(UserRegisterDto userRegisterDto, string password)
        {
            
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = userRegisterDto.Email,
                FirstName = userRegisterDto.FirstName,
                LastName = userRegisterDto.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            _userService.Add(user);

            using (var context = new FlowluContext())
            {
                
                var defaultRole = context.OperationClaims.FirstOrDefault(c => c.Name == "User");

                if (defaultRole != null)
                {
                    var userOperationClaim = new UserOperationClaim
                    {
                        UserId = user.Id,
                        OperationClaimId = defaultRole.Id
                    };
                    context.UserOperationClaims.Add(userOperationClaim);
                    context.SaveChanges();
                }
                
            }

            return new SuccessDataResult<User>(user, Messages.SuccessfulRegistered);
        }

        public IResult UserExist(string email)
        {
            if (_userService.GetUserByEmail(email) != null)
            {
                return new ErrorResult(Messages.UserAlreadyExist);
            }
            return new SuccessResult();
        }
    }
}
