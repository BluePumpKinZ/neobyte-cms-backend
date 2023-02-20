using System.Linq;

namespace Neobyte.Cms.Backend.Utils; 

public class PathUtils {

	public string Combine (params string[] paths) {
		return '/' + paths.Select(path => path.Trim().Trim('/'))
			.Aggregate((a, b) => string.Join('/', a, b));
	}

}