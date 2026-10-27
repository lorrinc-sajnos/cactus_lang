using CactusLang.Model.CodeStructure.MemberHolders;
using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.FileContent;

public class FileField(VariableSymbol symbol, CactusFile cactusFile) : MemberField(symbol, cactusFile) {
    public CactusFile CactusFile { get; } = cactusFile;

    public override string CName => CactusFile.ID.CPrefix + "_" + Name;
}