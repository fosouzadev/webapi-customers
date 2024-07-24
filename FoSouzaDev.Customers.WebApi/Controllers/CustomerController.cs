using FoSouzaDev.Customers.WebApi.Domain.DataTransferObjects;
using FoSouzaDev.Customers.WebApi.Domain.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FoSouzaDev.Customers.WebApi.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
public sealed class CustomerController(ICustomerService customerService) : ControllerBase
{
    [HttpPost]
    public async Task<Results<BadRequest<string>, Created<string>>> AddAsync(AddCustomerDto customer)
    {
        string id = await customerService.AddAsync(customer);
        return TypedResults.Created($"/{id}", id);
    }

    [HttpGet("{id}")]
    public async Task<Results<NotFound, Ok<CustomerDto>>> GetByIdAsync([FromRoute] string id)
    {
        CustomerDto customer = await customerService.GetByIdAsync(id);
        return TypedResults.Ok(customer);
    }

    [HttpPatch("{id}")]
    public async Task<Results<BadRequest<string>, NotFound, NoContent>> EditAsync([FromRoute] string id, [FromBody] EditCustomerDto customer)
    {
        await customerService.EditAsync(id, customer);
        return TypedResults.NoContent();
    }

    [HttpDelete]
    public async Task<Results<NotFound, NoContent>> DeleteAsync([FromRoute] string id)
    {
        await customerService.DeleteAsync(id);
        return TypedResults.NoContent();
    }
}