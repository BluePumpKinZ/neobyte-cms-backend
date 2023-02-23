namespace Neobyte.Cms.Backend.Core.Exceptions.RemoteHosting; 

public class FileAlreadyExistsException : AlreadyExistsException {

	public FileAlreadyExistsException (string? message) : base (message) {}

}