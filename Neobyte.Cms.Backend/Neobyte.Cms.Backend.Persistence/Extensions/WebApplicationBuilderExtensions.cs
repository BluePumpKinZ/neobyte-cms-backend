using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Neobyte.Cms.Backend.Core.Ports.Persistence.Repositories;
using Neobyte.Cms.Backend.Persistence.Adapters.Repositories;
using Neobyte.Cms.Backend.Persistence.Configuration;
using Neobyte.Cms.Backend.Persistence.EF;
using Neobyte.Cms.Backend.Persistence.EF.Initializer;

namespace Neobyte.Cms.Backend.Persistence.Extensions;

public static class WebApplicationBuilderExtensions {

	public static WebApplicationBuilder AddPersistence (this WebApplicationBuilder builder) {

		builder.Services.AddScoped<IReadOnlyAccountRepository, ReadOnlyAccountRepository>();
		builder.Services.AddScoped<IWriteOnlyAccountRepository, WriteOnlyAccountRepository>();

		builder.Services.AddScoped<IReadOnlyPageRepository, ReadOnlyPageRepository>();
		builder.Services.AddScoped<IWriteOnlyPageRepository, WriteOnlyPageRepository>();

		builder.Services.AddScoped<IReadOnlyWebsiteRepository, ReadOnlyWebsiteRepository>();
		builder.Services.AddScoped<IWriteOnlyWebsiteRepository, WriteOnlyWebsiteRepository>();

		// database configuration
		var dbConfig = new DatabaseConfig();
		builder.Configuration.GetSection("Database").Bind(dbConfig);
		builder.Services.AddDbContext<DbContext, EFDbContext>(opt => {
			opt.UseSqlServer(dbConfig.ConnectionString ?? throw new NullReferenceException(nameof(dbConfig.ConnectionString)));
		});

		builder.Services.AddScoped<DbContextInitializer>();
		builder.Services.AddSingleton<DbContextInitializerData>();
		return builder;
	}

}