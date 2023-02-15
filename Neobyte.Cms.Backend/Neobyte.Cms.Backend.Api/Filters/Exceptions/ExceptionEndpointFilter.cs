using Microsoft.Extensions.Logging;
using Neobyte.Cms.Backend.Core.Exceptions.Persistence;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Api.Filters.Exceptions;

public class ExceptionEndpointFilter : IEndpointFilter {

	private readonly ILogger<ExceptionEndpointFilter> _logger;

	public ExceptionEndpointFilter (ILogger<ExceptionEndpointFilter> logger) {
		_logger = logger;
	}

	public async ValueTask<object?> InvokeAsync (EndpointFilterInvocationContext context, EndpointFilterDelegate next) {

		try {
			return next.Invoke(context);
		} catch (NotFoundException e) {
			return Results.NotFound(new { e.Message });
		} catch (ApplicationException e) {
			_logger.LogError(e, "Application exception");
			return Results.BadRequest(new { e.Message });
		}
	}

}