using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.FileContent;

public class FileField(VariableSymbol symbol, CactusFile cactusFile) : ModelField(symbol) {
    public CactusFile CactusFile { get; } = cactusFile;

    public override string CName => CactusFile.FileID.CPrefix + "_" + Name;
}