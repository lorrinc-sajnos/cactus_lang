using CactusLang.Compiling.Codefiles;

namespace CactusLang.Model.Symbols;

public class FileID {
	public string Filename { get; }
	public string[] Folders { get; }

	public string Extention;


	internal FileID(string[] folders, string filename) {
		Filename = Path.GetFileNameWithoutExtension(filename);
		
		Extention = Path.GetExtension(filename);
		if (Extention == "")
			Extention = ".ccts";			
		Folders = folders;
	}

	public static FileID FromSrcFile(SourceFile file, CactusGrove grove) {
		string relPath = Path.GetRelativePath(grove.RootFolder, file.Path);

		string[] parts = relPath.Split('\\', '/');
		
		for (int i = 0; i < parts.Length; i++) 
			parts[i] = parts[i].Trim();

		string filename = Path.GetFileName(parts[^1]);
		
		string[] folders = parts[..^1];
		
		return new FileID(folders, filename);
	}

	public override bool Equals(object? obj) {
		if (obj is not FileID otherFileId)
			return base.Equals(obj);
		
		if (Filename != otherFileId.Filename)
			return false;
		if (Folders.Length != otherFileId.Folders.Length)
			return false;
		if  (Extention != otherFileId.Extention)
			return false;
		for (int i=0; i<Folders.Length; i++)
			if (Folders[i] != otherFileId.Folders[i])
				return false;
		
		return true;
	}

	public string AsRelPath() => String.Join('/', Folders) + Filename + Extention;
	public override string ToString() => AsRelPath();

	public string CPrefix => String.Join("_$_", Folders) + Filename + "_$$_";
}