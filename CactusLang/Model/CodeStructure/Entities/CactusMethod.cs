using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.Entities;

public abstract class CactusMethod(
    CactusFile cactusFile,
    FunctionSymbol symbol,
    IEntity parent
) : CactusFunction(cactusFile, symbol) {
    public IEntity Parent { get; } = parent;
}