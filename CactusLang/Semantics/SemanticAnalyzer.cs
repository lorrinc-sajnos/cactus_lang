using System.Runtime.InteropServices.JavaScript;
using CactusLang.Compiling.Codefiles;
using CactusLang.Model;
using CactusLang.Model.Symbols;
using CactusLang.Semantics.Errors;

namespace CactusLang.Semantics;

public class SemanticAnalyzer(CactusGrove grove) {
	public CactusGrove Grove { get; } = grove;
	public List<ErrorInstance> Errors = new();
	public bool HasErrors { get; set; } = false;

	private Dictionary<FileID, SemanticFileAnalyzer> _analyzers = new();

	public bool AnalyzeGrove() {
		//Start at root
		var rootResult = AnalyzeFile(Grove.RootFile);
		
		foreach (var analyzer in _analyzers.Values) {
			var fileErrors = analyzer.ErrorHandler.GetErrors();
			if (fileErrors.Count > 0) {
				HasErrors = true;
				Errors.AddRange(fileErrors);
			}
		}

		if (HasErrors) {
			PrintErrors();
		}
		return HasErrors;
	}

	public void PrintErrors() {
		foreach (var err in  Errors)
			Console.WriteLine(err.AsPrettyString());
	}
	

	bool AnalyzeFile(CactusFile file) {
		var fileAnalyzer = new SemanticFileAnalyzer(this, file);
		
		_analyzers.Add(file.FileID, fileAnalyzer);
		try {
			fileAnalyzer.AnalyzeFile();
		}
		catch (Exception e) {
			return false;
		}
		return true;
	}
	
	/// <summary>
	/// Handles the request for a file. If not yet read, reads it from the disk, then analyzes it.
	/// If file doesn't exist, returns null
	/// </summary>
	/// <param name="fileId"></param>
	/// <returns></returns>
	public CactusFile? RequestFile(FileID fileId) {
		CactusFile? file = Grove.GetFile(fileId);

		//If file is already read, just return
		if (file != null)
			return file;
		//Read new file:
		file = Grove.AddFileFromID(fileId);
		
		AnalyzeFile(file);

		return file;
	}
}