using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure;

public class ModelField {
    private readonly VariableSymbol _symbol;

    public ModelField(VariableSymbol symbol) {
        this._symbol = symbol;
    }
    
    public string Name => _symbol.Name;
    public VariableSymbol Symbol => _symbol;
    public BaseType Type => _symbol.Type;
}