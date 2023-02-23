using System;

namespace Neobyte.Cms.Backend.Core.Exceptions.RemoteHosting; 

public class AlreadyExistsException : ApplicationException {

	public AlreadyExistsException (string? message) : base (message) {}

}