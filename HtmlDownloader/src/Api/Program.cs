using Api.Dtos;
using Api.Mappers;
using Application;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure;
using Infrastructure.Redis;
using MassTransit;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RedisConfig>(builder.Configuration.GetSection("Redis"));

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.MapPost("/seed", async (DownloadLinkRequest request, IBus messageBus, ILinkVisitTracker linkVisitTracker) =>
{
    var link = LinkFactory.Create(request.Uri);
    if (link is null)
    {
        return Results.BadRequest($"{request.Uri} is not a valid uri");
    }

    CancellationTokenSource cancellationTokenSource = new();

    await linkVisitTracker.TrackLink(link, cancellationTokenSource.Token);

    var contract = request.MapToContract();
    await messageBus.Publish(contract);

    return Results.Created($"/downloadLinks/{contract.Id}", request);
})
.WithName("LinkSeeder")
.WithOpenApi();

app.Run();