using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.MemberHolders.IncludedFiles;
using CactusLang.Model.Symbols;

namespace CactusLang.Model;

public class IncludeManager(CactusFile parentFile) {
	private CactusFile _parentFile = parentFile;
	
	private Dictionary<string, IncludedFile> _includedFiles = new();

	public IncludedFile? GetIncludedFile(string name) => _includedFiles.GetValueOrDefault(name);

	public List<IncludedFile> IncludedFiles => _includedFiles.Values.ToList();
	public void AddIncludedFile(string alias, CactusFile file) {
		var incFile = new IncludedFile(file, alias, _parentFile);
		_includedFiles.Add(alias, incFile);
	}
	//public VariableSymbol Get
}