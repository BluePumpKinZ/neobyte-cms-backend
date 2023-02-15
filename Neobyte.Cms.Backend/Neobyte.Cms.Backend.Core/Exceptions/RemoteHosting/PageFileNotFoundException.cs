using System;

namespace Neobyte.Cms.Backend.Core.Exceptions.RemoteHosting; 

public class PageFileNotFoundException : ApplicationException {

	public PageFileNotFoundException (string? message) : base (message) {}

}