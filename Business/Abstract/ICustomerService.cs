using Core.Utilities.Results;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ICustomerService
    {
        IDataResult<List<Customer>> GetCustomers();
        IDataResult<Customer> GetCustomer(int customerId);
        IDataResult<Customer> CreateCustomer(Customer customer);
        IResult Update(Customer customer);
        IResult Delete(int customerId);
    }
}
