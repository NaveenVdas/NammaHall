using FluentValidation;
using NammaHall.Api.Configuration;
using NammaHall.Infrastructure;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.ToString());
});

builder.Host.UseDefaultServiceProvider((context, options) =>
{
    options.ValidateScopes = !context.HostingEnvironment.IsProduction();
    options.ValidateOnBuild = !context.HostingEnvironment.IsProduction();
});

builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.AddCors(builder.Configuration);
builder.Services.RegisterNammaHallServices(builder.Configuration);

builder.Services.AddHealthChecks();
if (builder.Configuration.GetSection("ApplicationInsights").Exists())
{
    builder.Services.AddApplicationInsightsTelemetry();
}

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

// Disable HTTPS redirection on Render (Render handles HTTPS at load balancer)
// Only use HTTPS redirection in development
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthz");

app.Run();

