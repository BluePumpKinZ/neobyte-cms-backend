using System.Threading.Tasks;

namespace Neobyte.Cms.Backend.Api.Extensions; 

public static class ResultExtensionsExtensions {

	public static IResult Html (this IResultExtensions results, string text) {
		return new HtmlResult(text);
	}

	private class HtmlResult : IResult {

		private readonly string _content;

		public HtmlResult (string content) {
			_content = content;
		}

		public Task ExecuteAsync (HttpContext httpContext) {
			httpContext.Response.Headers.Remove("Content-Type");
			httpContext.Response.Headers.Add("Content-Type", "text/html");
			return httpContext.Response.WriteAsync(_content);
		}

	}

}