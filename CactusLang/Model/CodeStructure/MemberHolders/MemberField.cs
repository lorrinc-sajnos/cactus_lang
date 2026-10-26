using CactusLang.Model.CodeStructure.MemberHolders.Structs;
using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.MemberHolders;

public class MemberField(VariableSymbol symbol, IMemberHolder parent) : ModelField(symbol) {
    public IMemberHolder Parent { get; } = parent;
    
    override public string CName => Parent.MemberType.CName+"_"+Name;
}