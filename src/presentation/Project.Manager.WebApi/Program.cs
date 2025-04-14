using Asp.Versioning;
using Project.Manager.Application;
using Project.Manager.Infra.Data;
using Project.Manager.WebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthorization();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1);
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'V";
        options.SubstituteApiVersionInUrl = true;
    });

builder.Services.AddEndpoints();
builder.Services.AddApplication();
builder.Services.AddInfraestructureData(builder.Configuration);
builder.Services.AddProblemDetails();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

var apiVersionSet = app.NewApiVersionSet()
                        .HasApiVersion(new ApiVersion(1))
                        .HasApiVersion(new ApiVersion(2))
                        .ReportApiVersions()
                        .Build();

RouteGroupBuilder group = app.MapGroup("api/v{version:apiVersion}")
    .WithApiVersionSet(apiVersionSet);

app.MapEndpoints(group);

app.Run();
