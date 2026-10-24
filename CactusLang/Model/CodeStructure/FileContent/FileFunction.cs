using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.FileContent;

public class FileFunction : ModelFunction {
    public FileFunction(FunctionSymbol symbol, CactusFile cactusFile) : base(cactusFile, symbol) {
        CactusFile = cactusFile;
        base.symbol = symbol;
    }

    public override string CName =>  CactusFile.FileID.CPrefix+Name;
}