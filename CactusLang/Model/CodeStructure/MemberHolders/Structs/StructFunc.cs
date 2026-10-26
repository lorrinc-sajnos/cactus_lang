using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.MemberHolders.Structs;

public class StructFunction(CactusFile file, FunctionSymbol symbol, FileStruct parent)
    : MemberFunction(file, symbol, parent) {
    private readonly FileStruct _parent = parent;
    public new IMemberHolder Parent => _parent;
    public FileStruct FileStruct => _parent;
}