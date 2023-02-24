using System.Diagnostics.Contracts;
using System.Linq;

namespace Neobyte.Cms.Backend.Utils;

public class PathUtils {

	[Pure]
	public string Combine (params string[] paths) {
		if (paths.Length == 0)
			return "";
		var parts = paths.Select(path => path.Trim().Trim('/'))
		.Where(s => !string.IsNullOrWhiteSpace(s));
		var joined = string.Join('/', parts);
		return CollapseSlashes ('/' + joined);
	}

	[Pure]
	public string GetPathAbove (string path) {
		path = CollapseSlashes(path);
		path = path.Trim().Trim('/');
		var parts = path.Split('/');
		if (parts.Length == 0)
			return "";
		return Combine(parts.Take(parts.Length - 1).ToArray());
	}

	[Pure]
	private string CollapseSlashes (string path) {
		// remove duplicate slashes
		return path.Replace("//", "/");
	}

}