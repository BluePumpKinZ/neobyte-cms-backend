using Neobyte.Cms.Backend.Domain.Websites;
using System.Text.RegularExpressions;

namespace Neobyte.Cms.Backend.Core.Websites.Transformers; 

public partial class CmsEditableHtmlTransformer : IHtmlTransformer {

	private static readonly Regex HTMLCmsEditableRegex = GetHtmlCmsEditableRegex();
	[GeneratedRegex("(<.* class=\".*cms-editable.*\")()(.*>)", RegexOptions.Compiled)]
	private static partial Regex GetHtmlCmsEditableRegex ();

	public bool Applies (TransformMode mode) {
		return mode == TransformMode.Render;
	}

	public string Up (Website website, string content) {
		content = content.Replace("cms-editable", "cms-editable mce-content-body cms-initialized");
		return HTMLCmsEditableRegex.Replace(content, m => m.Groups[1] + " contenteditable=\"true\"" + m.Groups[3]);
	}

	public string Down (string content) {
		return content.Replace("cms-editable mce-content-body cms-initialized", "cms-editable")
			.Replace("contenteditable=\"true\"", "");
	}

}