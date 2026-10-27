using CactusLang.Compiling.Codefiles;
using CactusLang.Model;
using CactusLang.Model.CodeStructure.FileContent;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang;

public class CactusGrove {
	public bool CompletedAnalysis { get; private set; }
	
	public string RootFolder { get; }
	public CactusFile RootFile { get; }
	public string RootFilePath { get; }


	private FunctionSymbol _entryPointSymb;
	
	
	public string SrcOutputFolder { get; }
	public string OutputFilePath { get; }
	
	private readonly Dictionary<FileID, CactusFile> _files;

	public CactusGrove(string rootFilePath) {
		RootFolder = Path.GetDirectoryName(rootFilePath)!;
		
		RootFilePath  = rootFilePath;
		SourceFile srcFile = new(rootFilePath);

		_files = new Dictionary<FileID, CactusFile>();
		RootFile = new CactusFile(this, srcFile);

		//Default entrypoint: function called main, with 0 args, returning an integer.
		_entryPointSymb = new FunctionSymbol(PrimitiveType.I32, "main", RootFile);
		
		SrcOutputFolder = RootFolder;
		OutputFilePath = DefaultOutputName(rootFilePath);
	}

	public CactusFile AddFile(CactusFile file) {
		_files.Add(file.ID, file);
		return file;
	}

	public CactusFile AddFileFromID(FileID fileID ) {
		//Read in file from disk00
		string absolutePath = RootFilePath+PathFromID( fileID);
		SourceFile srcFile = new SourceFile(absolutePath);
		
		return AddFile(new CactusFile(this, srcFile));
	}
	
	
	public CactusFile? GetFile(FileID fileId) {
		if (_files.TryGetValue(fileId, out var file)) 
			return file;
		return null;
	}
	
	public CactusFile[] GetFiles() => [.. _files.Values];
	
	static string DefaultOutputName(string inputFile) {
		string srcDir = Path.GetDirectoryName(inputFile)!;

		var os = Environment.OSVersion.Platform;
		string fileExtention = ".out";
		
		if (os == PlatformID.Win32NT) {
			fileExtention = ".exe";
		} else if (os == PlatformID.Unix) {
			fileExtention = ".out";
		}
		
		return $"{srcDir}/{Path.GetFileNameWithoutExtension(inputFile)}{fileExtention}";
	}
	
	public string PathFromID(FileID fileID) { 
		return string.Join("/", fileID.Folders) +  "/" + fileID.Filename + fileID.Extension ;
	}

	public FileFunction GetEntryPoint() => RootFile.Functions.GetExactFunction(_entryPointSymb)!;
}