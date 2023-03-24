using System;

namespace Neobyte.Cms.Backend.Core.Exceptions.Persistence; 

public class AccountIsAdministratorException : ApplicationException {
	public AccountIsAdministratorException (string? message) : base (message) {}

	
}