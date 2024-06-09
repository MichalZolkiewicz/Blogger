using Application.Dto.Posts;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using NLog.Web;
using OData.Swagger.Services;
using WebAPI.Installers;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.InstallServicesInAssembly(builder.Configuration);             

builder.Services.AddControllers().AddOData(
    options =>
    {
        options.Select().Filter().OrderBy().SetMaxTop(10).AddRouteComponents("odata", GetEdmModel());
        options.Conventions.Remove(options.Conventions.OfType<MetadataRoutingConvention>().First());
    });

builder.Services.AddOdataSwaggerSupport();
builder.Services.AddMemoryCache();
builder.Host.UseNLog();

var app = builder.Build();  

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) 
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapHealthChecksUI();


static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<PostDto>("Posts");
    return builder.GetEdmModel();
}

app.Run();

public partial class Program { }
