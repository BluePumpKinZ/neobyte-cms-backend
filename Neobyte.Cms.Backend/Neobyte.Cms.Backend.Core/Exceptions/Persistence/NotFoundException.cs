using System;

namespace Neobyte.Cms.Backend.Core.Exceptions.Persistence;

public abstract class NotFoundException : ApplicationException {

	public NotFoundException () {}

	public NotFoundException (string? message) : base (message) {}

	public NotFoundException (string? message, Exception? innerException) : base (message, innerException) {}

}