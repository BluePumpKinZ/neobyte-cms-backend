using System;

namespace Neobyte.Cms.Backend.Core.Exceptions.Persistence;

public abstract class AlreadyExistsException : ApplicationException {

	protected AlreadyExistsException () {}

	protected AlreadyExistsException (string? message) : base (message) {}

	protected AlreadyExistsException (string? message, Exception? innerException) : base (message, innerException) {}

}