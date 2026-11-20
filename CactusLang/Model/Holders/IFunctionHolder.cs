using CactusLang.Model.CodeStructure;
using CactusLang.Model.Symbols;

namespace CactusLang.Model.Holders;

public interface IFunctionHolder<FUNC> where FUNC : CactusFunction {
    public FUNC? GetExactFunction(FunctionSymbol id);
    public bool ContainsExactFunction(FuncCandidate id);

    public FUNC? GetExactFunction(FuncCandidate sign);

    public FUNC? GetMatchingFunction(FuncCandidate id);

    public bool ContainsMatchingFunction(FuncCandidate id);
    public FunctionSymbol? GetMatchingId(FuncCandidate signature);

    public List<FUNC> GetFunctions();
}