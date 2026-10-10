using CactusLang.Compiling.Codefiles;
using CactusLang.Model;
using CactusLang.Model.Symbols;

namespace CactusLang;

public class CactusGrove {
	public bool CompletedAnalysis { get; private set; }
	
	public string RootFolder { get; }
	public CactusFile RootFile { get; }
	public string RootFilePath { get; }
	
	
	public string SrcOutputFolder { get; }
	public string OutputFilePath { get; }
	
	private Dictionary<FileID, CactusFile> _files;

	public CactusGrove(string rootFilePath) {
		RootFolder = Path.GetDirectoryName(rootFilePath)!;
		
		RootFilePath  = rootFilePath;
		
		SourceFile srcFile = new(rootFilePath);
		
		_files = new Dictionary<FileID, CactusFile>();
		
		RootFile = new CactusFile(this, srcFile);

		
		SrcOutputFolder = RootFolder;

		OutputFilePath = DefaultOutputName(rootFilePath);
	}

	public CactusFile AddFile(CactusFile file) {
		_files.Add(file.FileID, file);
		return file;
	}

	public CactusFile? GetFile(FileID fileId) {
		if (_files.TryGetValue(fileId, out var file)) 
			return file;
		return null;
	}
	
	public CactusFile[] GetFiles() => [.. _files.Values];
	
	static string DefaultOutputName(string inputFile) {
		string srcDir = Path.GetDirectoryName(inputFile)!;
		return $"{srcDir}/{Path.GetFileNameWithoutExtension(inputFile)}.out";
	}
}