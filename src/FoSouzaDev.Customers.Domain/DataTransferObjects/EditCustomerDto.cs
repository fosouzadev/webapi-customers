﻿namespace FoSouzaDev.Customers.Domain.DataTransferObjects;

public sealed class EditCustomerDto
{
    public required string Name { get; init; }
    public required string LastName { get; init; }
    public string? Notes { get; init; }
}