using Neobyte.Cms.Backend.Persistence.Exceptions;

namespace Neobyte.Cms.Backend.Persistence.EF.Initializer;

internal class DbContextInitializer {

	private readonly DbContext _dbContext;
	private readonly DbContextInitializerData _data;

	public DbContextInitializer (DbContext dbContext, DbContextInitializerData data) {
		_dbContext = dbContext;
		_data = data;
	}

	public void Initialize () {
		if (_data.Initialized)
			throw new DbContextAlreadyInitializedException();
		
		_dbContext.Database.Migrate();
		
		_data.Initialized = true;
	}

}