using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neobyte.Cms.Backend.Api.Extensions;
using Neobyte.Cms.Backend.Core.Extensions;
using Neobyte.Cms.Backend.Identity.Extensions;
using Neobyte.Cms.Backend.Monitoring.Extensions;
using Neobyte.Cms.Backend.Persistence.Extensions;
using Neobyte.Cms.Backend.Utils.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApi();
builder.AddCore();
builder.AddIdentity(opt => {
	opt.Password.RequireDigit = true;
	opt.Password.RequireLowercase = true;
	opt.Password.RequireUppercase = true;
	opt.Password.RequireNonAlphanumeric = true;
	opt.Password.RequiredLength = 8;

	opt.User.RequireUniqueEmail = true;
	opt.SignIn.RequireConfirmedEmail = true;
});
builder.AddMonitoring();
builder.AddPersistence();
builder.AddUtils();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseApi();
app.UsePersistence();
await app.UseIdentity();
app.UseMonitoring();


if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Neobyte Cms Backend API"));
}

app.MapGet("/", () => "Hello World!");

app.Run();
