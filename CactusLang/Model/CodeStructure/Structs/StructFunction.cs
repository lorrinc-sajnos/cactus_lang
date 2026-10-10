using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.Structs;

public class StructFunction(CactusFile cactusFile, FunctionSymbol symbol, FileStruct parent) : ModelFunction(cactusFile, symbol) {
    public FileStruct Parent { get; } = parent;
}