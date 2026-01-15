using ChatOps.Api.Extensions;
using ChatOps.Api.Features;
using ChatOps.Api.Integrations;

var builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsProduction())
{
    builder.Configuration.AddUserSecrets<Program>(optional: true);
}

builder.AddIntegrations();
builder.AddAdapters();
builder.AddFeatures();

var app = builder.Build();

app.Run();