using CactusLang.Model;
using CactusLang.Model.Symbols;

namespace CactusLang.Semantics;

public class SemanticAnalyzer {
	public CactusGrove Grove { get; }

	public SemanticAnalyzer(CactusGrove grove) {
		Grove = grove;
	}

	/// <summary>
	/// Handles the request for a file. If not yet read, reads it from the disk.
	/// If file doesn't exist, returns null
	/// </summary>
	/// <param name="fileId"></param>
	/// <returns></returns>
	public CactusFile? RequestFile(FileID fileId) {
		CactusFile? file = Grove.GetFile(fileId);

		if (file != null)
			return file;
		
		
	}
}