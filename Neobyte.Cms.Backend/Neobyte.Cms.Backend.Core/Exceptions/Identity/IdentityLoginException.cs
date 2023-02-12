using System;

namespace Neobyte.Cms.Backend.Core.Exceptions.Identity;

public class IdentityLoginException : IdentityException {

	public IdentityLoginException (string? message, Exception? innerException) : base (message, innerException) {}

}