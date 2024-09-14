using Business.Abstract;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CustomerManager : ICustomerService
    {
        private ICustomerDal _customerDal;
        private IProjectDal _projectDal;
        private ITaskDal _taskDal;
        private ITeamMemberDal _teamMemberDal;
        private ITeamDal _teamDal;
        private IUserDal _userDal;
        private IHttpContextAccessor _httpContextAccessor;

        public CustomerManager(ICustomerDal customerDal, IHttpContextAccessor httpContextAccessor, IProjectDal projectDal, ITaskDal taskDal, ITeamMemberDal teamMemberDal, ITeamDal teamDal, IUserDal userDal)
        {
            _customerDal = customerDal;
            _projectDal = projectDal;
            _httpContextAccessor = httpContextAccessor;
            _taskDal = taskDal;
            _teamMemberDal = teamMemberDal;
            _teamDal = teamDal;
            _userDal = userDal;
        }

        public IDataResult<Customer> CreateCustomer(Customer customer)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _userDal.Get(u => u.Id == userId);
            var teamMember = _teamMemberDal.Get(tm => tm.UserEmail == user.Email);
            var team = _teamDal.Get(t => t.Id == teamMember.TeamId);

            customer.UserId = team.UserId;
            _customerDal.Add(customer);
            return new SuccessDataResult<Customer>(customer,Messages.CustomerAdded);
        }

        public IResult Delete(int customerId)
        {
            var customer = _customerDal.Get(c => c.Id == customerId);
            if (customer == null)
            {
                return new ErrorResult(Messages.CustomerNotFound);
            }

            var projects = _projectDal.GetList(p => p.CustomerId == customerId).ToList();

            if (projects.Count != 0)
            {
                var taskId = projects[0].Id;
                var tasks = _taskDal.GetList(t => t.ProjectId == taskId).ToList();
                tasks.ForEach(t => _taskDal.Delete(t));
                projects.ForEach(p => _projectDal.Delete(p));
            }

            _customerDal.Delete(customer);
            return new SuccessResult(Messages.CustomerDeleted);
            
        }

        public IDataResult<List<Customer>> GetCustomers()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var user = _userDal.Get(u => u.Id == userId);
            var customers = new List<Customer>();
            var userTeam = _teamMemberDal.Get(tm => tm.UserEmail == user.Email);
            if (userTeam != null)
            {
                var team = _teamDal.Get(t => t.Id == userTeam.TeamId);
                customers = _customerDal.GetList(c => c.UserId == team.UserId).ToList();
            }
            else
            {
                customers = _customerDal.GetList(c=>c.UserId == userId);
            }

            return new SuccessDataResult<List<Customer>>(customers);
        }

        public IDataResult<Customer> GetCustomer(int customerId)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            return new SuccessDataResult<Customer>(_customerDal.Get(c => c.Id == customerId  && c.UserId == userId));
        }

        public IResult Update(Customer customer)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _userDal.Get(u => u.Id == userId);
            var teamMember = _teamMemberDal.Get(tm => tm.UserEmail == user.Email);
            var team = _teamDal.Get(t => t.Id == teamMember.TeamId);

            customer.UserId = team.UserId;
            _customerDal.Update(customer);
            return new SuccessResult(Messages.CustomerUpdated);
        }
    }
}
