using CactusLang.Model.CodeStructure.Entities;
using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.FileContent;

public class FileVariable(VariableSymbol symbol, CactusFile cactusFile) : CactusField(symbol, cactusFile) {
    public CactusFile CactusFile { get; } = cactusFile;
}