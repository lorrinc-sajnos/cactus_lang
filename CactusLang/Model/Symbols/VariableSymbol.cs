using CactusLang.Model.Types;
using CactusLang.Tags;

namespace CactusLang.Model.Symbols;

public class VariableSymbol {
    public string Name { get; }
    public string Id { get; }
    public CactusType Type { get; }

    //public VariableSymbol(CactusType type, string name) : this(type, 0, name) { }

    public VariableSymbol(VariableSymbol copy) {
        Name = copy.Name;
        Id = copy.Id;
        Type = copy.Type;
    }
    public VariableSymbol(CactusType type, string name) {
        Name = name;
        Type = type;
        Id = new(name);
    }
    
    public string CName => Name;
}