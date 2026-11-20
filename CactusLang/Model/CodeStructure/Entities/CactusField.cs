using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.Entities;

public class CactusField(VariableSymbol symbol, IEntity parent) : CactusVariable(symbol) {
    public IEntity Parent { get; } = parent;
    
    //override public string CName => Parent.MemberType.CName+"_"+Name;
}