using Neobyte.Cms.Backend.Domain.Websites;
using System.Text.RegularExpressions;

namespace Neobyte.Cms.Backend.Core.Websites.Transformers; 

public partial class TinyMCEHtmlTransformer : IHtmlTransformer {

	private static readonly Regex TinyMCEContentRegex = GetTinyMCEContentRegex();
	[GeneratedRegex("<tinymce-inject-first><\\/tinymce-inject-first>[\\S\\s]*<\\/body>", RegexOptions.Compiled)]
	private static partial Regex GetTinyMCEContentRegex ();

	private static readonly Regex TinyMCEStyleSheetsRegex = GetTinyMCEStyleSheetsRegex();
	[GeneratedRegex("<link .*? id=\"mce-u[0-9]+\" href=\".*?\">", RegexOptions.Compiled)]
	private static partial Regex GetTinyMCEStyleSheetsRegex ();

	private static readonly Regex TinyMCEMozBrokenRegex = GetTinyMCEMozBrokenRegex();
	[GeneratedRegex("<style id=\"mceDefaultStyles\" .*?>[\\S\\s]*?<\\/style>", RegexOptions.Compiled)]
	private static partial Regex GetTinyMCEMozBrokenRegex ();

	public bool Applies (TransformMode mode) {
		return mode == TransformMode.Render;
	}

	public string Up (Website website, string content) {
		return content;
	}

	public string Down (string content) {
		content = TinyMCEContentRegex.Replace(content, "</body>");
		content = TinyMCEStyleSheetsRegex.Replace(content, "");
		return TinyMCEMozBrokenRegex.Replace(content, "");
	}

}