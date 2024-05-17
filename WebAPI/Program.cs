using Application;
using Application.Dto.Posts;
using Application.Interfaces;
using Application.Services;
using Application.Services.Emails;
using Application.Validators;
using Cosmonaut;
using Cosmonaut.Extensions.Microsoft.DependencyInjection;
using Domain.Entities.Cosmos;
using FluentValidation.AspNetCore;
using HealthChecks.UI.Client;
using Infrastructure;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Routing.Conventions;
using Microsoft.Azure.Documents.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NLog.Web;
using OData.Swagger.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using WebAPI.Cache;
using WebAPI.HealthChecks;
using WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddFluentValidation(options =>
    {
        options.RegisterValidatorsFromAssemblyContaining<CreatePostDtoValidator>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.WriteIndented = true;
    })
    .AddXmlSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPI", Version = "v1" });
    c.ExampleFilters();
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, new string[] { }}
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services
    .AddFluentEmail(builder.Configuration["FluentEmail:FromEmail"], builder.Configuration["FluentEmail:FromName"])
    .AddRazorRenderer()
    .AddSmtpSender(() => new SmtpClient(builder.Configuration["FluentEmail:SmtpSender:Host"], int.Parse(builder.Configuration["FluentEmail:SmtpSender:Port"]))
    {
        EnableSsl = true,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(builder.Configuration["FluentEmail:SmtpSender:Username"], builder.Configuration["FluentEmail:SmtpSender:Password"])
    });
                    
builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddApiVersioning(x =>
{
    x.DefaultApiVersion = new ApiVersion(1, 0);
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.ReportApiVersions = true;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

var redisCacheSettings = new RedisCacheSettings();
builder.Configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
builder.Services.AddSingleton(redisCacheSettings);

if(!redisCacheSettings.Enabled)
{
    return;
}

builder.Services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
builder.Services.AddSingleton<IResponseCacheService, ResponseCacheService>();


builder.Services.AddControllers().AddOData(
    options =>
    {
        options.Select().Filter().OrderBy().SetMaxTop(10).AddRouteComponents("odata", GetEdmModel());
        options.Conventions.Remove(options.Conventions.OfType<MetadataRoutingConvention>().First());
    });

builder.Services.AddOdataSwaggerSupport();
builder.Services.AddSwaggerExamplesFromAssemblyOf<Program>();
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<BloggerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BloggerCS")));

var cosmosStoreSettings = new CosmosStoreSettings(
    builder.Configuration["CosmosSettings:DatabaseName"],
    builder.Configuration["CosmosSettings:AccountUri"],
    builder.Configuration["CosmosSettings:AccountKey"],
    new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp }
    );
builder.Services.AddCosmosStore<CosmosPost>(cosmosStoreSettings);

builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddHealthChecks()
    .AddDbContextCheck<BloggerContext>("Database");
builder.Services.AddHealthChecksUI()
    .AddInMemoryStorage();
builder.Services.AddHealthChecks()
    .AddCheck<ResponseTimeHealthCheck>("Network speed test");
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
