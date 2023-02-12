using System;

namespace Neobyte.Cms.Backend.Core.Exceptions.Websites; 

public class InvalidProtocolException : ApplicationException {

	public InvalidProtocolException (string? message) : base (message) {}

}