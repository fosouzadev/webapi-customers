﻿using Testcontainers.MongoDb;

namespace FoSouzaDev.Customers.CommonTests;

public sealed class MongoDbFixture : IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer = new MongoDbBuilder().WithImage("mongo:latest").Build();

    public MongoDbContainer MongoDbContainer => _mongoDbContainer;

    public Task InitializeAsync() =>
        this._mongoDbContainer.StartAsync();

    public Task DisposeAsync() =>
        this._mongoDbContainer.DisposeAsync().AsTask();
}