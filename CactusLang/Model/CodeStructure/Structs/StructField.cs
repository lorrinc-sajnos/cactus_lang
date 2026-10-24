using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.Structs;

public class StructField(VariableSymbol symbol, FileStruct parent) : ModelField(symbol) {
    public FileStruct Parent { get; } = parent;
    
    override public string CName => Parent.StructType.CName+"_"+Name;
}