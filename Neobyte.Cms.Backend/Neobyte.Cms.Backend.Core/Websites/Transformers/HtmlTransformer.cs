using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Text.RegularExpressions;

namespace Neobyte.Cms.Backend.Core.Websites.Transformers;

public partial class HtmlTransformer {

	private static readonly Regex HTMLHeadRegex = GetHtmlHeadRegex();
	private static readonly Regex HTMLCmsEditableRegex = GetHtmlCmsEditableRegex();

	private static readonly Regex HTMLBaseHrefRegex = GetHtmlBaseHrefRegex();
	private static readonly Regex CssStylingRegex = GetCssStylingRegex();

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
			_logger.LogWarning("Cms styles file not found");
			return;
		}

		_cssStyles = File.ReadAllText(_options.CmsStylesPath);
	}

	// Construction
	[GeneratedRegex("(.*<head>)()([\\S\\s]*)(<\\/head>.*)", RegexOptions.Compiled)]
	private static partial Regex GetHtmlHeadRegex ();
	[GeneratedRegex("(<.* class=\".*cms-editable.*\")()(.*>)", RegexOptions.Compiled)]
	private static partial Regex GetHtmlCmsEditableRegex ();

	private static string GenerateBaseHrefTag (string domain) {
		return $"<base href=\"{domain}/\">";
	}

	public string ConstructRenderedWebpage (string domain, string htmlContent) {
		htmlContent = HTMLHeadRegex.Replace(htmlContent, m => m.Groups[1] + GenerateBaseHrefTag(domain)
		+ m.Groups[3] + _cssStyles + m.Groups[5])
			.Replace("cms-editable", "cms-editable mce-content-body cms-initialized");

		return HTMLCmsEditableRegex.Replace(htmlContent, m => m.Groups[1] + " contenteditable=\"true\"" + m.Groups[3]);
	}

	// Deconstruction
	[GeneratedRegex("<base href=\".*\">", RegexOptions.Compiled)]
	private static partial Regex GetHtmlBaseHrefRegex ();
	[GeneratedRegex("(<style id=\"cms_style_k4U\".*>[\\S\\s]*?<\\/style>)([\\S\\s]*<\\/head>)", RegexOptions.Compiled)]
	private static partial Regex GetCssStylingRegex ();

	public string DeconstructRenderedWebPage (string htmlContent) {
		htmlContent = htmlContent.Replace("cms-editable mce-content-body cms-initialized", "cms-editable")
			.Replace("contenteditable=\"true\"", "");
		htmlContent = HTMLBaseHrefRegex.Replace(htmlContent, "");
		htmlContent = CssStylingRegex.Replace(htmlContent, m => "" + m.Groups[2]);
		return htmlContent;
	}

}