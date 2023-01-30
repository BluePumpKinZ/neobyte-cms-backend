using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Neobyte.Cms.Backend.Persistence.Exceptions;

namespace Neobyte.Cms.Backend.Persistence.EF.Initializer;

internal class DbContextInitializer {

	private readonly DbContext _dbContext;
	private readonly ILogger<DbContextInitializer> _logger;
	private readonly DbContextInitializerData _data;

	public DbContextInitializer (DbContext dbContext, ILogger<DbContextInitializer> logger, DbContextInitializerData data) {
		_dbContext = dbContext;
		_logger = logger;
		_data = data;
	}

	public void Initialize () {
		if (_data.Initialized)
			throw new DbContextAlreadyInitializedException();
		
		_dbContext.Database.Migrate();
			
		_data.Initialized = true;
	}

}