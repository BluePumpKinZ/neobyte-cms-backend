using System;

namespace Neobyte.Cms.Backend.Core.Exceptions.RemoteHosting; 

public class InvalidPathException : ApplicationException {

	public InvalidPathException (string? message) : base (message) {}

}