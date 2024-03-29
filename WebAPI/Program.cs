using Application;
using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using Domain.Entities.Cosmos;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Documents.Client;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(2, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
});
builder.Services.AddDbContext<BloggerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BloggerCS")));

var cosmosStoreSettings = new CosmosStoreSettings(
    builder.Configuration["CosmosSettings:DatabaseName"],
    builder.Configuration["CosmosSettings:AccountUri"],
    builder.Configuration["CosmosSettings:AccountKey"],
    new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp }
    );

builder.Services.AddCosmosStore<CosmosPost>(cosmosStoreSettings);

var app = builder.Build();  

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
