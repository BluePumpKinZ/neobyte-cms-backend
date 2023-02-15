using System.Text.RegularExpressions;

namespace Neobyte.Cms.Backend.Core.Websites; 

public class HtmlTransformer {

	private readonly Regex _htmlHeadRegex = new Regex ("(.*<head>)()([\\S\\s]*)(<\\/head>.*)", RegexOptions.Compiled);
	private readonly Regex _htmlCmsEditableRegex = new Regex("(<.* class=\".*cms-editable.*\")()(.*>)", RegexOptions.Compiled);
	
	private string GenerateBaseHrefTag(string domain) {
		return $"<base href=\"{domain}/\">";
	}

	public string TransformRenderedWebpage (string domain, string htmlContent) {
		htmlContent = _htmlHeadRegex.Replace(htmlContent, m => m.Groups[1] + GenerateBaseHrefTag(domain) + m.Groups[3] + m.Groups[5]);

		htmlContent = htmlContent.Replace("cms-editable", "cms-editable mce-content-body cms-initialized");

		return _htmlCmsEditableRegex.Replace(htmlContent, m => m.Groups[1] + " contenteditable=\"true\" " + m.Groups[3]);
	}

}