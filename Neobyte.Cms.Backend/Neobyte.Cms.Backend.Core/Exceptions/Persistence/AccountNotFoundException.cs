namespace Neobyte.Cms.Backend.Core.Exceptions.Persistence;

public class AccountNotFoundException : NotFoundException {

	public AccountNotFoundException (string? message) : base (message) {}

}