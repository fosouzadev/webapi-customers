using FoSouzaDev.Customers.WebApi.Domain.DataTransferObjects;
using FoSouzaDev.Customers.WebApi.Domain.Entities;
using FoSouzaDev.Customers.WebApi.Domain.Services;
using FoSouzaDev.Customers.WebApi.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FoSouzaDev.Customers.WebApi.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Route("api/v1/customer")]
public sealed class CustomerController(ICustomerService customerService) : ControllerBase
{
    [HttpPost]
    public async Task<Results<BadRequest<ResponseData<string>>, Created<ResponseData<string>>>> AddAsync(AddCustomerDto customer)
    {
        string id = await customerService.AddAsync(customer);
        return TypedResults.Created($"api/v1/customer/{id}", new ResponseData<string>(data: id));
    }

    [HttpGet("{id}")]
    public async Task<Results<NotFound, Ok<ResponseData<CustomerDto>>>> GetByIdAsync([FromRoute] string id)
    {
        Customer? customer = await customerService.GetByIdAsync(id);

        return TypedResults.Ok(new ResponseData<CustomerDto>(new CustomerDto 
        {
            Id = customer!.Id,
            Name = customer.FullName.Name,
            LastName = customer.FullName.LastName,
            BirthDate = customer.BirthDate.Date,
            Email = customer.Email.Value,
            Notes = customer.Notes
        }));
    }

    [HttpPatch("{id}")]
    public async Task<Results<BadRequest<ResponseData<string>>, NotFound, NoContent>> EditAsync([FromRoute] string id, [FromBody] EditCustomerDto customer)
    {
        await customerService.EditAsync(id, customer);
        return TypedResults.NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<Results<NotFound, NoContent>> DeleteAsync([FromRoute] string id)
    {
        await customerService.DeleteAsync(id);
        return TypedResults.NoContent();
    }
}