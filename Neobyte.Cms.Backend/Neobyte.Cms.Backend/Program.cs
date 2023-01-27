using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neobyte.Cms.Backend.Api.Extensions;
using Neobyte.Cms.Backend.Monitoring.Extensions;
using Neobyte.Cms.Backend.Persistence.Extensions;
using Neobyte.Cms.Backend.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApi();
builder.AddMonitoring();
builder.AddPersistence();
builder.AddUtils();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UsePersistence();

if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Neobyte Cms Backend API"));
}

app.MapGet("/", () => "Hello World!");

app.Run();
