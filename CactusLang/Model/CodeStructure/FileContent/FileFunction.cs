using CactusLang.Model.CodeStructure.MemberHolders;
using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.FileContent;

public class FileFunction : MemberFunction {
    public FileFunction(FunctionSymbol symbol, CactusFile cactusFile) : base(cactusFile, symbol, cactusFile) {
        CactusFile = cactusFile;
        base.symbol = symbol;
    }

}