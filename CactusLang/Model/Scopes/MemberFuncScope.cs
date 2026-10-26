using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.MemberHolders;
using CactusLang.Model.CodeStructure.MemberHolders.Structs;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.Scopes;

public class MemberFuncScope : Scope {
    private readonly IMemberHolder _mHolder;
    private VariableSymbol _thisPtr;

    public MemberFuncScope(FileStruct mHolder) {
        _mHolder = mHolder;
        _thisPtr = new VariableSymbol(_mHolder.MemberType.GetPointer(), "this");

        InitVariables();
    }

    private void InitVariables() {
        foreach (var varSymbol in _mHolder.GetFields()) {
            var result = AddVariable(new MemberFieldRefSymbol(_mHolder, varSymbol.Symbol));
        }
    }

    public override ModelFunction? GetMatchingFunction(FuncSignature signature) {
        if (_mHolder.ContainsMatchingFunction(signature))
            return _mHolder.GetMatchingFunction(signature);

        return base.GetMatchingFunction(signature);
    }
}