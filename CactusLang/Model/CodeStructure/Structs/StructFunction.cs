using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.Structs;

public class StructFunction(CodeFile file, FunctionSymbol symbol, FileStruct parent) : ModelFunction(file, symbol) {
    public FileStruct Parent { get; } = parent;
}