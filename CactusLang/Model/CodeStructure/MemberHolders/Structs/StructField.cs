using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.MemberHolders.Structs;

public class StructField(VariableSymbol symbol, CactusStruct parent) : MemberField(symbol, parent) {
    public CactusStruct CactusStruct => parent;
}