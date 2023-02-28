namespace Neobyte.Cms.Backend.Utils.Tests;

public class PathUtilsTests {

	private readonly PathUtils _pathUtils;

	public PathUtilsTests () {
		_pathUtils = new PathUtils();
	}

	[Fact]
	public void Combine_ShouldInsertSlashesBetweenPaths () {
		var combined = _pathUtils.Combine("abc", "defg", "hijkl");
		Assert.Equal("/abc/defg/hijkl", combined);
	}

	[Fact]
	public void Combine_ShouldAddALeadingSlash () {
		var combined = _pathUtils.Combine("a");
		Assert.Equal("/a", combined);
	}

	[Fact]
	public void Combine_ShouldTrimExistingSlashes () {
		var combined = _pathUtils.Combine("a", "b", "/c", "d/", "/e/");
		Assert.Equal("/a/b/c/d/e", combined);
	}

	[Fact]
	public void Combine_ShouldReturnEmptyStringForNoInput () {
		var combined = _pathUtils.Combine();
		Assert.Equal("", combined);
	}

	[Fact]
	public void Combine_ShouldIgnoreEmptyPaths () {
		var combined = _pathUtils.Combine("a", "", "b", "");
		Assert.Equal("/a/b", combined);
	}

	[Fact]
	public void Combine_ShouldHandlePathsWithWhitespaceOnly () {
		var combined = _pathUtils.Combine("  ", "\t", "  ", "\n");
		Assert.Equal("/", combined);
	}


	[Fact]
	public void GetPathAbove_ShouldReturnEmptyStringForRootPath () {
		const string path = "/";
		var pathAbove = _pathUtils.GetPathAbove(path);
		Assert.Equal("", pathAbove);
	}

	[Fact]
	public void GetPathAbove_ShouldReturnParentDirectoryPathForFilePath () {
		const string path = "/dir1/dir2/file.txt";
		var pathAbove = _pathUtils.GetPathAbove(path);
		Assert.Equal("/dir1/dir2", pathAbove);
	}

	[Fact]
	public void GetPathAbove_ShouldReturnParentDirectoryPathForDirectoryPath () {
		const string path = "/dir1/dir2/";
		var pathAbove = _pathUtils.GetPathAbove(path);
		Assert.Equal("/dir1", pathAbove);
	}

	[Fact]
	public void GetPathAbove_ShouldHandlePathsWithConsecutiveSlashesCorrectly () {
		const string path = "/dir1//dir2/file.txt";
		var pathAbove = _pathUtils.GetPathAbove(path);
		Assert.Equal("/dir1/dir2", pathAbove);
	}

	[Fact]
	public void GetPathAbove_ShouldHandlePathsWithLeadingOrTrailingSlashesCorrectly () {
		const string path2 = "/dir1/dir2/file.txt/";
		var pathAbove2 = _pathUtils.GetPathAbove(path2);
		Assert.Equal("/dir1/dir2", pathAbove2);
	}



}