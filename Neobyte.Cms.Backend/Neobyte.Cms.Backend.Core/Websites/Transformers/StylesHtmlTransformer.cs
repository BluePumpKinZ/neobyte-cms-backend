using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neobyte.Cms.Backend.Domain.Websites;
using System.IO;
using System.Text.RegularExpressions;

namespace Neobyte.Cms.Backend.Core.Websites.Transformers; 

public partial class StylesHtmlTransformer : IHtmlTransformer {

	private static readonly Regex HTMLHeadRegex = GetHtmlHeadRegex();
	[GeneratedRegex("(.*<head>[\\S\\s]*)()(<\\/head>.*)", RegexOptions.Compiled)]
	private static partial Regex GetHtmlHeadRegex ();
	private static readonly Regex CssStylingRegex = GetCssStylingRegex();
	[GeneratedRegex("<style id=\"cms_style_k4U\".*>[\\S\\s]*?<\\/style>", RegexOptions.Compiled)]
	private static partial Regex GetCssStylingRegex ();

	private readonly HtmlTransformerOptions _options;
	private readonly ILogger<HtmlTransformer> _logger;
	private string _cssStyles = "";

	public StylesHtmlTransformer (IOptions<HtmlTransformerOptions> options, ILogger<HtmlTransformer> logger) {
		_options = options.Value;
		_logger = logger;
		LoadCssStyles();
	}

	private void LoadCssStyles () {
		if (!File.Exists(_options.CmsStylesPath)) {
			_logger.LogWarning("Cms styles file not found");
			return;
		}

		_cssStyles = File.ReadAllText(_options.CmsStylesPath);
	}

	public bool Applies (TransformMode mode) {
		return mode == TransformMode.Render;
	}

	public string Up (Website website, string content) {
		return HTMLHeadRegex.Replace(content, m => m.Groups[1] + _cssStyles + m.Groups[3]);
	}

	public string Down (string content) {
		return CssStylingRegex.Replace(content, "");
	}

}