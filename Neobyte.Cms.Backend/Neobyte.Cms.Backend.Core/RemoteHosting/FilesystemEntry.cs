﻿using System;

namespace Neobyte.Cms.Backend.Core.RemoteHosting;

public class FilesystemEntry {

	public string Name { get; set; }
	public string Path { get; set; }
	public bool IsDirectory { get; set; }
	public long? Size { get; set; }
	public DateTime LastModified { get; set; }

	public FilesystemEntry (string name, string path, bool isDirectory, long? size, DateTime lastModified) {
		Name = name;
		Path = path;
		IsDirectory = isDirectory;
		Size = size;
		LastModified = lastModified;
	}

}