using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.MemberHolders;

public abstract class MemberFunction(
    CactusFile cactusFile,
    FunctionSymbol symbol,
    IMemberHolder parent
) : ModelFunction(cactusFile, symbol) {
    public IMemberHolder Parent { get; } = parent;

    public override string CName => Name;
}