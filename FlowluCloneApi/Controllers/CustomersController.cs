using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlowluCloneApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin,Manager,User")]
    public class CustomersController : ControllerBase
    {
        private ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("getcustomers")]
        public IActionResult GetCustomers()
        {
            var result = _customerService.GetCustomers();
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }
        [HttpGet("getcustomer/{id}")]
        public IActionResult GetCustomer(int id)
        {
            var result = _customerService.GetCustomer(id);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("createcustomer")]
        [Authorize(Roles ="Admin,User")]
        public IActionResult CreateCustomer(Customer customer)
        {
            var result = _customerService.CreateCustomer(customer);
            if (result.Success)
            {
                return Ok(result.Data);
            }
            return BadRequest(result.Message);
        }

        [HttpPost("delete/{id}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Delete(int id)
        {
            var result = _customerService.Delete(id);
            if (result.Success)
            {
                return Ok( new { message = result.Message });
            }
            return BadRequest(result.Message);
        }

        [HttpPost("update")]
        public IActionResult Update(Customer customer)
        {
            var result = _customerService.Update(customer);
            if (result.Success)
            {
                return Ok(new { message = result.Message });
            }
            return BadRequest(result.Message);
        }
    }
}
