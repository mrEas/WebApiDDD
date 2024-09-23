using App.Application.Customers.Commands;
using App.Application.Customers.Queries;
using App.Domain.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [ApiController]
    [Route("customers")]
    public class CustomerController : ApiControllerBase
    {
        private readonly ISender _sender;
        public CustomerController(ISender sender)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerCommand createCustomerCommand)
        {
            var result = await _sender.Send(createCustomerCommand);
            return result.Match(customerId => Ok(customerId), errors => Problem(errors));
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerCommand updateCustomerCommand)
        {
            var result = await _sender.Send(updateCustomerCommand);
            return result.Match(customerId => Ok(customerId), errors => Problem(errors));
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomer(string customerId)
        {
            if (!Guid.TryParse(customerId, out var guidId))
            {
                return BadRequest("Invalid customer ID");
            }

            var customerIdValue = new CustomerId(guidId);
            var query = new DeleteCustomerCommand(customerIdValue);
            var result = await _sender.Send(query);

            return result.Match(customerId => Ok(customerId), errors => Problem(errors)); 
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var query = new GetAllCustomersQuery();
            var result = await _sender.Send(query);

            return Ok(result);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomer(string customerId)
        {
            if (!Guid.TryParse(customerId, out var guidId))
            {
                return BadRequest("Invalid customer ID");
            }

            var customerIdValue = new CustomerId(guidId);
            var query = new GetCustomerQuery(customerIdValue);
            var result = await _sender.Send(query);

            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("exists/{customerId}")]
        public async Task<IActionResult> IsCustomerExist(string customerId)
        {
            if (!Guid.TryParse(customerId, out var guidId))
            {
                return BadRequest("Invalid customer ID");
            }
            var customerIdValue = new CustomerId(guidId);
            var query = new IsCustomerExistQuery(customerIdValue);
            var result = await _sender.Send(query);

            if (!result)
                return NotFound();

            return Ok(result);
        }

    }
}
