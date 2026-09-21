using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Tags;

namespace CactusLang.Model.CodeStructure;

public abstract class ModelFunction {
    public TagContainer TagContainer { get; }

    protected FunctionSymbol _symbol;
    protected CodeBlock _codeBody;

    protected CodeFile _codeFile;
    public CodeFile CodeFile => _codeFile;

    public ModelFunction(CodeFile file, FunctionSymbol symbol) {
        _symbol = symbol;
        _codeBody = new CodeBlock(this);
        _codeFile = file;
        TagContainer = new();
    }
    public CodeBlock CodeBody => _codeBody;
    public FunctionSymbol Symbol => _symbol;
    public string Name => _symbol.Name;
    public BaseType ReturnType => _symbol.ReturnType;
    public FuncId FuncId => _symbol.Id;
    
}