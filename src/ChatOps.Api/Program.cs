using ChatOps.Api.Adapters.Http;
using ChatOps.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsProduction())
{
    builder.Configuration.AddUserSecrets<Program>(optional: true);
}

builder.Services.RegisterServices(builder.Configuration);

var app = builder.Build();

app.MapEndpoints();

app.Run();