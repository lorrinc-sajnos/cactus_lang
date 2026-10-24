using CactusLang.Model.CodeStructure;
using CactusLang.Model.Symbols;

namespace CactusLang.Model;

public class IncludeManager {
	public string Alias { get; }
	
	private Dictionary<FileID, CactusFile> _includedFiles = new();

	public ModelFunction? GetMatchingFunction(FuncSignature singature) {
		throw new NotImplementedException();
	} 
	
	public VariableSymbol Get
}