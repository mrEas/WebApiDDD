using App.Application.Customers.Commands;
using App.Application.Customers.Queries;
using App.Domain.Customers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace App.Api.Controllers
{
    [Route("customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ISender _sender;

        public CustomerController(ISender sender)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var query = new GetAllCustomersQuery();
            var result = await _sender.Send(query);
            return Ok(result);
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomer(CustomerId customerId)
        {
            var query = new GetCustomerQuery(customerId);
            var result = await _sender.Send(query);

            if (result is null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomer(CustomerId customerId)
        {
            var query = new DeleteCustomerCommand(customerId);
            var result = await _sender.Send(query);

            if (result.IsError)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCustomerCommand createCustomerCommand)
        {
            var result = await _sender.Send(createCustomerCommand);
            if (result.IsError)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result);
        }
    }
}
