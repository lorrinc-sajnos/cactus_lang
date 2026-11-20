using CactusLang.Model.CodeStructure.Entities;
using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.FileContent;

public class FileFunction : CactusMethod {
    public FileFunction(FunctionSymbol symbol, CactusFile cactusFile) : base(cactusFile, symbol, cactusFile) {
        CactusFile = cactusFile;
        _symbol = symbol;
    }

}