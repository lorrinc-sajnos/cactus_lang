using CactusLang.Model.Types;
using CactusLang.Tags;

namespace CactusLang.Model.Symbols;

public class FunctionSymbol {
	public FileID FileID { get; }
	public string Name { get; }
	public BaseType ReturnType { get; }
	public OrderedDictionary<string, VariableSymbol> Parameters { get; }

	public FunctionSymbol(BaseType retType, string name, CactusFile owner) {
		FileID = owner.ID;
		Name = name;
		ReturnType = retType;
		Parameters = new();
	}

	public void AddParameter(VariableSymbol parameter) {
		Parameters.Add(parameter.Name, parameter);
	}

	public List<VariableSymbol> GetParameters() => Parameters.Values.ToList();

	public FuncSignature GetSignature() => new FuncSignature(Name, Parameters.Values.Select(v => v.Type).ToList());


	public bool ExactSignatureMatch(FuncSignature signature) {
		var thisSign = GetSignature();
		return thisSign.Equals(signature);
	}
	
	public bool DoesMatchSignature(FuncSignature filter) {
		if (Name != filter.Name) return false;
		if (Parameters.Count != filter.Params.Count) return false;

		for (int i = 0; i < Parameters.Count; i++) {
			var currParam = Parameters.GetAt(i).Value;
			var filterType = filter.Params[i];

			if (!currParam.Type.CanBeUsedAs(filterType)) return false;
		}

		return true;
	}

	public override bool Equals(object? obj) {
		if (!(obj is FunctionSymbol other)) return false;
		return Equals(other);
		
	}

	private bool Equals(FunctionSymbol other) {
		if (!FileID.Equals(other.FileID))
			return false;
		if (!Name.Equals(other.Name))
			return false;
		if (!ReturnType.Equals(other.ReturnType))
			return false;
		
		if (Parameters.Count != other.Parameters.Count)
			return false;
		for (int i=0;i<Parameters.Count;i++) {
			var thisParam = Parameters.GetAt(i).Value;
			var otherParam = other.Parameters.GetAt(i).Value;
			
			if (!thisParam.Equals(otherParam)) 
				return false;
		}
		return true;
	}

	public override int GetHashCode() {
		return HashCode.Combine(FileID, Name, ReturnType, Parameters);
	}
}