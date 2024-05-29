using Api.Dtos;
using Api.Mappers;
using Application;
using Application.RabbitMq;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

var rabbitMQConfig = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQConfig>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices(rabbitMQConfig!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/seed", async (DownloadLinkRequest request, IBus messageBus) =>
{
    var contract = request.MapToContract();

    await messageBus.Publish(contract);

    return Results.Created($"/downloadLinks/{contract.Id}", request);
})
.WithName("LinkSeeder")
.WithOpenApi();

app.Run();