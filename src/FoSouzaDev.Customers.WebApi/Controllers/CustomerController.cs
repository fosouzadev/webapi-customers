using FoSouzaDev.Customers.Domain.DataTransferObjects;
using FoSouzaDev.Customers.Domain.Entities;
using FoSouzaDev.Customers.Domain.Services;
using FoSouzaDev.Customers.WebApi.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FoSouzaDev.Customers.WebApi.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Route("api/v1/customer")]
public sealed class CustomerController(ICustomerService customerService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> AddAsync(AddCustomerDto customer)
    {
        string id = await customerService.AddAsync(customer);
        return TypedResults.Created($"api/v1/customer/{id}", new ResponseData<string>(data: id));
    }

    [HttpGet("{id}")]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> GetByIdAsync([FromRoute] string id)
    {
        Customer? customer = await customerService.GetByIdAsync(id);
        return TypedResults.Ok(new ResponseData<CustomerDto>((CustomerDto)customer!));
    }

    [HttpPatch("{id}")]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> EditAsync([FromRoute] string id, [FromBody] EditCustomerDto customer)
    {
        await customerService.EditAsync(id, customer);
        return TypedResults.NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> DeleteAsync([FromRoute] string id)
    {
        await customerService.DeleteAsync(id);
        return TypedResults.NoContent();
    }
}