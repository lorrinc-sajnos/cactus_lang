using System.Runtime.CompilerServices;
using System.Text;
using CactusLang.Compiling;
using CactusLang.Model.Types;

namespace CactusLang.Model.Symbols;

public readonly struct FuncSignature(string name, List<BaseType> parameters, BaseType retType) {
    public FuncSignature(string s, List<VariableSymbol> toList, BaseType returnType) :
        this(s, toList.Select(v => v.Type).ToList(), returnType) {
    }

    public string Name { get; } = name;
    public List<BaseType> Params { get; } = parameters;
    public BaseType ReturnType { get; } = retType;


    public void AddParam(BaseType param) {
        Params.Add(param);
    }


    
    public bool ExactMatch(FuncCandidate cand) {
        if (!Name.Equals(cand.Name)) return false;
        if (Params.Count != cand.Params.Count) return false;

        for (int i = 0; i < Params.Count; i++) {
            if (!Params[i].Equals(cand.Params[i])) return false;
        }
        
        if (cand.ReturnType!=null && ReturnType != cand.ReturnType) return false;

        return true;
    }
    
    public bool DoesMatch(FuncCandidate cand) {
        if (!Name.Equals(cand.Name)) return false;
        if (Params.Count != cand.Params.Count) return false;

        for (int i = 0; i < Params.Count; i++) {
            if (!Params[i].CanBeUsedAs(cand.Params[i])) return false;
        }
        
        if (cand.ReturnType!=null && ReturnType != cand.ReturnType) return false;

        return true;
    }

    public bool Equals(FuncSignature other) {
        if (Name != other.Name) return false;
        if (Params.Count != other.Params.Count) return false;

        for (int i = 0; i < Params.Count; i++)
            if (!Params[i].Equals(other.Params[i]))
                return false;

        if (ReturnType != other.ReturnType) return false;

        return true;
    }

    public override bool Equals(object? obj) {
        return obj is FuncSignature other && Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(Name, Params);
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
        //sb.Append("->"+ReturnType);
        return sb.ToString();
    }
}

public readonly struct FuncCandidate(string name, List<BaseType> parameters, BaseType? retType=null) {
    public string Name { get; } = name;
    public List<BaseType> Params { get; } = parameters;
    public BaseType? ReturnType { get; } = retType;
}