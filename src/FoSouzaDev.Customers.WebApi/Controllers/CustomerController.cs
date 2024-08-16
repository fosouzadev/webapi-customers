using FoSouzaDev.Customers.Application.DataTransferObjects;
using FoSouzaDev.Customers.Application.Services;
using FoSouzaDev.Customers.WebApi.Responses;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace FoSouzaDev.Customers.WebApi.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Route("api/v1/customer")]
public sealed class CustomerController(ICustomerApplicationService customerApplicationService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status201Created)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> AddAsync(AddCustomerDto customer)
    {
        string id = await customerApplicationService.AddAsync(customer);
        return TypedResults.Created($"api/v1/customer/{id}", new ResponseData<string>(data: id));
    }

    [HttpGet("{id}")]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> GetByIdAsync([FromRoute] string id)
    {
        CustomerDto? customer = await customerApplicationService.GetByIdAsync(id);
        return TypedResults.Ok(new ResponseData<CustomerDto>(customer!));
    }

    [HttpPatch("{id}")]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> EditAsync([FromRoute] string id, [FromBody] JsonPatchDocument<EditCustomerDto> pathDocument)
    {
        await customerApplicationService.EditAsync(id, pathDocument);
        return TypedResults.NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status204NoContent)]
    [ProducesResponseType<ResponseData<string>>(StatusCodes.Status500InternalServerError)]
    public async Task<IResult> DeleteAsync([FromRoute] string id)
    {
        await customerApplicationService.DeleteAsync(id);
        return TypedResults.NoContent();
    }
}