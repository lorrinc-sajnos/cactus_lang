using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.Entities.Structs;

public class StructMethod(CactusFile file, FunctionSymbol symbol, CactusStruct parent)
    : CactusMethod(file, symbol, parent) {
    //public new IEntity Parent => _parent;
    public CactusStruct CactusStruct { get; } = parent;
}