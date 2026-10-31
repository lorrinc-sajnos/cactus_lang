using CactusLang.Model.CodeStructure;
using CactusLang.Model.Symbols;

namespace CactusLang.Model.Holders;

public interface IFunctionHolder<FUNC> where FUNC : ModelFunction {
    public FUNC? GetExactFunction(FunctionSymbol id);
    public bool ContainsExactFunction(FuncSignature id);

    public FUNC? GetExactFunction(FuncSignature sign);

    public FUNC? GetMatchingFunction(FuncSignature id);

    public bool ContainsMatchingFunction(FuncSignature id);
    public FunctionSymbol? GetMatchingId(FuncSignature signature);

    public List<FUNC> GetFunctions();
}