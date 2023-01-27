using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Neobyte.Cms.Backend.Api.Extensions;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Monitoring.Extensions;
using Neobyte.Cms.Backend.Persistence.Extensions;
using Neobyte.Cms.Backend.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApi();
builder.AddMonitoring();
builder.AddPersistence();
builder.AddUtils();

var app = builder.Build();

app.UsePersistence();

app.MapGet("/", () => "Hello World!");

app.Run();
