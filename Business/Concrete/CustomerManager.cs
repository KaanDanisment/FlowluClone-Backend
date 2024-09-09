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
        private IHttpContextAccessor _httpContextAccessor;

        public CustomerManager(ICustomerDal customerDal,IHttpContextAccessor httpContextAccessor, IProjectDal projectDal, ITaskDal taskDal)
        {
            _customerDal = customerDal;
            _projectDal = projectDal;
            _httpContextAccessor = httpContextAccessor;
            _taskDal = taskDal;
        }

        public IDataResult<Customer> CreateCustomer(Customer customer)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            customer.UserId = int.Parse(userId);
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
                projects.ForEach(p => _projectDal.Delete(p));
                var taskId = projects[0].Id;
                var tasks = _taskDal.GetList(t => t.ProjectId == taskId).ToList();
                tasks.ForEach(t => _taskDal.Delete(t));
            }

            _customerDal.Delete(customer);
            return new SuccessResult(Messages.CustomerDeleted);
            
        }

        public IDataResult<List<Customer>> GetCustomers()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var customers = _customerDal.GetList(c=>c.UserId == userId);

            return new SuccessDataResult<List<Customer>>(customers);
        }

        public IDataResult<Customer> GetCustomer(int customerId)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            return new SuccessDataResult<Customer>(_customerDal.Get(c => c.Id == customerId  && c.UserId == userId));
        }

        public IResult Update(Customer customer)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            customer.UserId = userId;
            _customerDal.Update(customer);
            return new SuccessResult(Messages.CustomerUpdated);
        }
    }
}
