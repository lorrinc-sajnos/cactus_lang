using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Tags;

namespace CactusLang.Model.CodeStructure;

public abstract class ModelFunction {
    protected CodeBlock _codeBody;
    public CodeBlock CodeBody => _codeBody;
    
    protected FunctionSymbol symbol;
    public FunctionSymbol Symbol => symbol;
    
    public TagContainer TagContainer { get; }
    public CactusFile CactusFile { get; protected set; }
    public string Name => symbol.Name;
    public BaseType ReturnType => symbol.ReturnType;

    
    public ModelFunction(CactusFile cactusFile, FunctionSymbol symbol) {
        this.symbol = symbol;
        _codeBody = new CodeBlock(this);
        CactusFile = cactusFile;
        TagContainer = new();
    }
    
    public abstract string CName { get; }
}