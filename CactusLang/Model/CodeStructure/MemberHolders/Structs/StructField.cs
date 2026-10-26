using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.MemberHolders.Structs;

public class StructField(VariableSymbol symbol, FileStruct parent) : MemberField(symbol, parent) {
    public new IMemberHolder Parent => parent;
    public FileStruct FileStruct => parent;
}