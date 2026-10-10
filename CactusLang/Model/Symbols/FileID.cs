using CactusLang.Compiling.Codefiles;

namespace CactusLang.Model.Symbols;

public class FileID {
	public string Filename { get; }
	private string[] _folders;

	private FileID(string[] folders, string filename ) {
		Filename = filename;
		_folders = folders;
	}

	public static FileID FromSrcFile(SourceFile file, CactusGrove grove) {
		string relPath = Path.GetRelativePath(grove.RootFolder, file.Path);
	}
	
	public string AsString() { 
		//TODO
		return "TODO";
	}
}