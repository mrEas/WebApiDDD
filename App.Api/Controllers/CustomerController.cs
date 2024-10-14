using App.Application.Customers.Commands;
using App.Application.Customers.Queries;
using App.Domain.Customers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace App.Api.Controllers
{
    [ApiController]
    [Route("customers")]
    [Authorize]
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
            return result.Match(customerId => Ok(result), errors => Problem(errors));
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerCommand updateCustomerCommand)
        {
            var result = await _sender.Send(updateCustomerCommand);
            return result.Match(_ => NoContent(), errors => Problem(errors));
        }

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> DeleteCustomer(Guid customerId)
        {
            var query = new DeleteCustomerCommand(customerId);
            var result = await _sender.Send(query);

            return result.Match(_ => NoContent(), errors => Problem(errors));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var query = new GetAllCustomersQuery();
            var result = await _sender.Send(query);

            return result.Match(customers => Ok(customers), errors => Problem(errors));
        }

        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomer(Guid customerId)
        {
            var query = new GetCustomerQuery(customerId);
            var result = await _sender.Send(query);

            return result.Match(customer => Ok(customer), errors => Problem(errors));
        }

        [HttpGet("exists/{customerId}")]
        public async Task<IActionResult> IsCustomerExist(Guid customerId)
        {
            var query = new IsCustomerExistQuery(customerId);
            var result = await _sender.Send(query);

            return result.Match(customer => Ok(customer), errors => Problem(errors));
        }

    }
}
