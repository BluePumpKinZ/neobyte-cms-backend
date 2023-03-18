using System;
using System.Diagnostics.Contracts;
using System.IO;
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
		return Combine(parts.Take(parts.Length - 1).ToArray());
	}

	[Pure]
	private string CollapseSlashes (string path) {
		// remove duplicate slashes
		return path.Replace("//", "/");
	}

	[Pure]
	public string GetS3Path (string path) {
		path = CollapseSlashes(path);
		if (path.StartsWith("/")) {
			return path[1..];
		}
		return path;
	}

	[Pure]
	public string GetS3DirectoryFromPath (string key, string path) {
		key = GetS3Path(key);
		path = GetS3Path(path);
		string[] keyParts = key.Split('/');
		int directoryIndex = Array.IndexOf(keyParts, path);
		if (directoryIndex >= 0 && directoryIndex < keyParts.Length - 1) {
			return keyParts[directoryIndex + 1];
		}

		return key.Split("/")[0] + "/";
	}
}