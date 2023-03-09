using Neobyte.Cms.Backend.Domain.Websites;

namespace Neobyte.Cms.Backend.Core.Websites.Transformers;

public class HtmlTransformer {

	private readonly IEnumerable<IHtmlTransformer> _transformers;

	public HtmlTransformer (IEnumerable<IHtmlTransformer> transformers) {
		_transformers = transformers;
	}

	public string ConstructRenderedWebpage (Website website, string htmlContent, TransformMode mode) {

		foreach (var transformer in _transformers) {
			if (!transformer.Applies(mode))
				continue;

			htmlContent = transformer.Up(website, htmlContent);
		}

		return htmlContent;
		
	}

	public string DeconstructRenderedWebPage (string htmlContent, TransformMode mode) {
		
		foreach (var transformer in _transformers) {
			if (!transformer.Applies(mode))
				continue;

			htmlContent = transformer.Down(htmlContent);
		}

		return htmlContent;
		
	}

}