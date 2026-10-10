using System.Text;
using CactusLang.Compiling;
using CactusLang.Model.Types;

namespace CactusLang.Model.Symbols;

public struct FuncID : IEquatable<FuncID> {
	public string Name { get; }

	public List<BaseType> Params { get; }

	public CactusFile CactusFile { get; }

	public void AddParam(BaseType param) {
		Params.Add(param);
	}

	public FuncID(string name, CactusFile cactusFile, params BaseType[] parameters)
		: this(name, cactusFile, new List<BaseType>(parameters)) {
	}

	public FuncID(string name, CactusFile cactusFile, List<BaseType> parameters) {
		Name = name;
		Params = parameters;
		CactusFile = cactusFile;
	}


	public bool DoesMatch(FuncID filter) {
		if (Name != filter.Name) return false;
		if (Params.Count != filter.Params.Count) return false;

		for (int i = 0; i < Params.Count; i++) {
			if (!Params[i].CanBeUsedAs(filter.Params[i])) return false;
		}

		return true;
	}


	public override string ToString() {
		StringBuilder sb = new();
		sb.Append($"{Name}(");
		for (int i = 0; i < Params.Count - 1; i++) {
			sb.Append($"{Params[i]}, ");
		}

		if (Params.Count != 0)
			sb.Append($"{Params[^1]}");
		sb.Append(')');
		return sb.ToString();
	}

	public override bool Equals(object? obj) {
		if (obj is not FuncID funcId)
			return false;

		if (!Name.Equals(funcId.Name))
			return false;

		if (Params.Count != funcId.Params.Count)
			return false;

		for (int i = 0; i < Params.Count; i++) {
			if (!Params[i].Equals(funcId.Params[i]))
				return false;
		}


		return true;
	}

	public bool Equals(FuncID other) {
		return Params.Equals(other.Params);
	}

	public override int GetHashCode() {
		return HashCode.Combine(Params, Name);
	}
}