using Api.Dtos;
using Api.Mappers;
using Application;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/seed", async (DownloadLinkRequest request, IPublishEndpoint publishEndpoint) =>
{
    var contract = request.MapToContract();

    await publishEndpoint.Publish(contract);

    return Results.Created($"/downloadLinks/{contract.Id}", request);
})
.WithName("LinkSeeder")
.WithOpenApi();

app.Run();