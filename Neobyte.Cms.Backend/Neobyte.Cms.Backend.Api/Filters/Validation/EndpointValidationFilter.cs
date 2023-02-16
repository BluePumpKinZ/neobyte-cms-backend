using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Api.Filters.Validation;

public class EndpointValidationFilter<T> : IEndpointFilter {

	public ValueTask<object?> InvokeAsync (EndpointFilterInvocationContext context, EndpointFilterDelegate next) {

		var body = context.Arguments.SingleOrDefault(a => a is T);
		try {
			if (body is null)
				throw new ValidationException("Request body cannot be empty");

			Validator.ValidateObject(body, new ValidationContext(body), true);
			return next(context);
		} catch (ValidationException ex) {
			return ValueTask.FromResult((object?)Results.BadRequest(ex.Message));
		}
	}

}