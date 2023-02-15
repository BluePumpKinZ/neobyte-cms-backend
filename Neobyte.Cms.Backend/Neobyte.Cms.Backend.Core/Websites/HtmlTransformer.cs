using System.Text.RegularExpressions;

namespace Neobyte.Cms.Backend.Core.Websites; 

public class HtmlTransformer {

	private readonly Regex _htmlHeadRegex = new Regex ("(.*<head>)()([\\S\\s]*)(<\\/head>.*)", RegexOptions.Compiled);
	
	private string GenerateBaseHrefTag(string domain) {
		return $"<base href=\"{domain}/\">";
	}

	public string TransformRenderedWebpage (string domain, string htmlContent) {
		return _htmlHeadRegex.Replace(htmlContent, m => m.Groups[1] + GenerateBaseHrefTag(domain) + m.Groups[3] + m.Groups[5]);
	}

}