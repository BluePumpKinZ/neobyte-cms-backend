using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text.RegularExpressions;

namespace Neobyte.Cms.Backend.Core.Websites.Transformers;

public partial class HtmlTransformer {

	private static readonly Regex HTMLHeadRegex = GetHtmlHeadRegex();
	private static readonly Regex HTMLCmsEditableRegex = GetHtmlCmsEditableRegex();

	private readonly HtmlTransformerOptions _options;
	private readonly ILogger<HtmlTransformer> _logger;
	private string _cssStyles = "";

	public HtmlTransformer (IOptions<HtmlTransformerOptions> options, ILogger<HtmlTransformer> logger) {
		_options = options.Value;
		_logger = logger;
		LoadCssStyles();
	}

	private void LoadCssStyles () {
		if (!File.Exists(_options.CmsStylesPath)) {
			_logger.LogWarning("Cms styles file not found.");
			return;
		}

		_cssStyles = File.ReadAllText(_options.CmsStylesPath);
	}

	private static string GenerateBaseHrefTag (string domain) {
		return $"<base href=\"{domain}/\">";
	}

	public string TransformRenderedWebpage (string domain, string htmlContent) {
		htmlContent = HTMLHeadRegex.Replace(htmlContent, m => m.Groups[1] + GenerateBaseHrefTag(domain)
		+ m.Groups[3] + _cssStyles + m.Groups[5])
			.Replace("cms-editable", "cms-editable mce-content-body cms-initialized");

		return HTMLCmsEditableRegex.Replace(htmlContent, m => m.Groups[1] + " contenteditable=\"true\" " + m.Groups[3]);
	}

	[GeneratedRegex("(.*<head>)()([\\S\\s]*)(<\\/head>.*)", RegexOptions.Compiled)]
	private static partial Regex GetHtmlHeadRegex ();
	[GeneratedRegex("(<.* class=\".*cms-editable.*\")()(.*>)", RegexOptions.Compiled)]
	private static partial Regex GetHtmlCmsEditableRegex ();

}