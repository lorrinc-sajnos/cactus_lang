using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.MemberHolders.Structs;

public class StructFunction(CactusFile file, FunctionSymbol symbol, CactusStruct parent)
    : MemberFunction(file, symbol, parent) {
    //public new IMemberHolder Parent => _parent;
    public CactusStruct CactusStruct { get; } = parent;
}