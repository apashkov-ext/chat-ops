using ChatOps.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsProduction())
{
    builder.Configuration.AddUserSecrets<Program>(optional: true);
}

builder.RegisterServices();

var app = builder.Build();

app.MapFeatures();

app.Run();