using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure;

public abstract class ModelField(VariableSymbol symbol) {
    public string Name => symbol.Name;
    public VariableSymbol Symbol => symbol;
    public BaseType Type => symbol.Type;
    
    public abstract string CName { get; }
}