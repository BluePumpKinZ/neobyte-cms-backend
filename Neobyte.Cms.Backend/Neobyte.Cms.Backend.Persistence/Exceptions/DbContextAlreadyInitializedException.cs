using System;

namespace Neobyte.Cms.Backend.Persistence.Exceptions;

public class DbContextAlreadyInitializedException : ApplicationException {
	
	public DbContextAlreadyInitializedException () : base("Database already initialized") { }

}