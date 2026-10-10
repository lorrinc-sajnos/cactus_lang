using CactusLang.Model.Types;
using CactusLang.Tags;

namespace CactusLang.Model.Symbols;

public class FunctionSymbol {
	public string Name { get; private set; }

	public FuncID Id { get; private set; }
	public BaseType ReturnType { get; private set; }

	private readonly Dictionary<string, VariableSymbol> _parameters;

	public FunctionSymbol(BaseType retType, string name, CactusFile owner) {
		Name = name;
		ReturnType = retType;
		_parameters = new();

		var parameterTypes = _parameters.Select(p => p.Value.Type).ToList();
		Id = new FuncID(Name, owner, parameterTypes);
	}

	public void AddParameter(VariableSymbol parameter) {
		_parameters.Add(parameter.Name, parameter);
		Id.AddParam(parameter.Type);
	}

	public List<VariableSymbol> GetParameters() => _parameters.Values.ToList();
}