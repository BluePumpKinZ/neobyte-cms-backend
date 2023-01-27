using Microsoft.AspNetCore.Builder;
using Neobyte.Cms.Backend.Api.Extensions;
using Neobyte.Cms.Backend.Monitoring.Extensions;
using Neobyte.Cms.Backend.Persistence.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApi();
builder.AddMonitoring();
builder.AddPersistence();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
