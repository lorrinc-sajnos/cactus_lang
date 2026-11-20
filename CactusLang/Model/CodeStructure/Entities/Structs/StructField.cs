using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.Entities.Structs;

public class StructField(VariableSymbol symbol, CactusStruct parent) : CactusField(symbol, parent) {
    public CactusStruct CactusStruct => parent;
}