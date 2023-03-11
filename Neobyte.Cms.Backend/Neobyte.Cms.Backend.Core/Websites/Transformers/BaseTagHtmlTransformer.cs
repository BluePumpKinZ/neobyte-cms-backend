using Neobyte.Cms.Backend.Domain.Websites;
using System.Text.RegularExpressions;

namespace Neobyte.Cms.Backend.Core.Websites.Transformers; 

public partial class BaseTagHtmlTransformer : IHtmlTransformer {

	private static readonly Regex HTMLHeadRegex = GetHtmlHeadRegex();
	[GeneratedRegex("(.*<head>)()([\\S\\s]*<\\/head>.*)", RegexOptions.Compiled)]
	private static partial Regex GetHtmlHeadRegex ();

	private static readonly Regex HTMLBaseHrefRegex = GetHtmlBaseHrefRegex();
	[GeneratedRegex("<base href=\".*\">", RegexOptions.Compiled)]
	private static partial Regex GetHtmlBaseHrefRegex ();

	public bool Applies (TransformMode mode) {
		return mode is TransformMode.Display or TransformMode.Render;
	}

	public string Up (Website website, string content) {
		var domain = website.Domain;
		return HTMLHeadRegex.Replace(content, m => m.Groups[1] + GenerateBaseHrefTag(domain) + m.Groups[3]);
	}

	public string Down (string content) {
		return HTMLBaseHrefRegex.Replace(content, "");
	}

	private static string GenerateBaseHrefTag (string domain) {
		return $"<base href=\"{domain}/\">";
	}

}