using System;
using System.Runtime.Serialization;

namespace Neobyte.Cms.Backend.Core.Exceptions; 

public abstract class IdentityException : ApplicationException {

	protected IdentityException () {}

	protected IdentityException (SerializationInfo info, StreamingContext context) : base (info, context) {}

	protected IdentityException (string? message) : base (message) {}

	protected IdentityException (string? message, Exception? innerException) : base (message, innerException) {}

}