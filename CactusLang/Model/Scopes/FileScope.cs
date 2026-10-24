using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Semantics;
using CactusLang.Semantics.Errors;

namespace CactusLang.Model.Scopes;

public class FileScope : Scope {
    private readonly CactusFile _cactusFile;
    private Scope _currentScope;
    private readonly ErrorHandler _errorHandler;
    public Scope CurrentScope => _currentScope;

    public FileScope(CactusFile cactusFile,ErrorHandler errorHandler) {
        _cactusFile = cactusFile;
        _errorHandler = errorHandler;
        _currentScope = this;
        //_fileFunctionSymbolStore = new();
    }

    public void StepIn() {
        Scope newScope = _currentScope.CreateChild();
        _currentScope = newScope;
    }

    public void StepInStructFunc(FileStruct fileStruct) {
        StructFuncScope newScope = new StructFuncScope(fileStruct);
        _currentScope.AddChild(newScope);
        _currentScope = newScope;
    }

    public void StepOut() {
        _currentScope = _currentScope!.Parent;
    }

    //public override FunctionSymbol? GetExactFunction(FuncId id) => _fileFunctionStore.GetExactFunction(id);

    public override ModelFunction? GetMatchingFunction(FuncSignature sign) => _cactusFile.Functions.GetMatchingFunction(sign);
    
    public override int GetScopeDepth() => 0;

    public override VariableSymbol? GetVariable(string id) {
        return base.GetVariable(id);
    }
}