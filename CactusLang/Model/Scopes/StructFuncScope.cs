using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.Scopes;

public class StructFuncScope : Scope {
    private readonly FileStruct _fileStruct;
    private VariableSymbol _thisPtr;

    public StructFuncScope(FileStruct fileStruct) {
        _fileStruct = fileStruct;
        _thisPtr = new VariableSymbol(_fileStruct.StructType.GetPointer(), "this");

        InitVariables();
    }

    private void InitVariables() {
        foreach (var varSymbol in _fileStruct.Fields.GetFields()) {
            var result = AddVariable(new StructFieldRefSymbol(_fileStruct, varSymbol.Symbol));
        }
    }

    public override ModelFunction? GetMatchingFunction(FuncSignature signature) {
        if (_fileStruct.Functions.ContainsMatchingFunction(signature))
            return _fileStruct.Functions.GetMatchingFunction(signature);

        return base.GetMatchingFunction(signature);
    }
}