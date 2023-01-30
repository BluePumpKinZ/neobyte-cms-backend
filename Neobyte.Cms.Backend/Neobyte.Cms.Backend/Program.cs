using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Neobyte.Cms.Backend.Api.Extensions;
using Neobyte.Cms.Backend.Core.Extensions;
using Neobyte.Cms.Backend.Identity.Extensions;
using Neobyte.Cms.Backend.Mailing.Extensions;
using Neobyte.Cms.Backend.Monitoring.Extensions;
using Neobyte.Cms.Backend.Persistence.Extensions;
using Neobyte.Cms.Backend.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("env.json", true);

builder.AddApi();
builder.AddCore();
builder.AddIdentity();
builder.AddMailing();
builder.AddMonitoring();
builder.AddPersistence();
builder.AddUtils();

var app = builder.Build();

app.UseApi();
app.UsePersistence();
app.UseIdentity();
app.UseMonitoring();

app.Run();
