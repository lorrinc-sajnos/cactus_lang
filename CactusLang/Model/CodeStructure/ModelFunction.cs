using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Tags;

namespace CactusLang.Model.CodeStructure;

public abstract class ModelFunction {
    protected CodeBlock _codeBody;
    public CodeBlock CodeBody => _codeBody;
    
    protected FunctionSymbol _symbol;
    public FunctionSymbol Symbol => _symbol;
    
    public TagContainer TagContainer { get; }
    public CactusFile CactusFile { get; protected set; }
    public string Name => _symbol.Name;
    public BaseType ReturnType => _symbol.ReturnType;
    public FuncID FuncId => _symbol.Id;
    
    public ModelFunction(CactusFile cactusFile, FunctionSymbol symbol) {
        _symbol = symbol;
        _codeBody = new CodeBlock(this);
        CactusFile = cactusFile;
        TagContainer = new();
    }
}