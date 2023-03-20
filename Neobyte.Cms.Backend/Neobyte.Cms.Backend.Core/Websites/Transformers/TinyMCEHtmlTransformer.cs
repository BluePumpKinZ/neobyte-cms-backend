using Neobyte.Cms.Backend.Domain.Websites;
using System.Text.RegularExpressions;

namespace Neobyte.Cms.Backend.Core.Websites.Transformers; 

public partial class TinyMCEHtmlTransformer : IHtmlTransformer {

	private static readonly Regex TinyMCEContentRegex = GetTinyMCEContentRegex();
	[GeneratedRegex("<tinymce-inject-first><\\/tinymce-inject-first>[\\S\\s]*<\\/body>", RegexOptions.Compiled)]
	private static partial Regex GetTinyMCEContentRegex ();

	public bool Applies (TransformMode mode) {
		return mode == TransformMode.Render;
	}

	public string Up (Website website, string content) {
		return content;
	}

	public string Down (string content) {
		return TinyMCEContentRegex.Replace(content, "</body>");
	}

}